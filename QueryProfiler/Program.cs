using Kusto.Language;
using Kusto.Language.Syntax;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace QueryProfiler
{
    class Program
    {

        public static string GetSubSymbolAndFindKey(string strToSub)
        {
            string[] symbolArr = { "Operator", "Condition", "Expression" };
            foreach (var symbol in symbolArr)
            {
                if(strToSub!=null)
                if (strToSub.Contains(symbol))
                {
                    return strToSub.Substring(0, strToSub.Length - symbol.Length);
                }
            }
            return null;
        }
        public static void GetProfile(string query)
        {
            var profileScheme = new ProfileScheme();
            var type = typeof(ProfileScheme);
            var propertiesProfileScheme = new Dictionary<string, int>();
            var code = KustoCode.Parse(query);
            var Operator = "Operator"; 
            var Expression = "Expression";
            var Condition = "Condition";
            string tempSubKey = null;
            foreach (PropertyInfo prop in type.GetProperties())
            {
                var subProp = prop.Name.Substring(0, prop.Name.Length - 7);
                propertiesProfileScheme.Add(subProp, (int)prop.GetValue(profileScheme));
            }
            var tempCount = 0;
            SyntaxElement.WalkNodes(code.Syntax,
           n =>
           {
               var tempKind = n.Kind.ToString();
               if (tempKind.Contains(Operator) || n.NameInParent == Expression || n.NameInParent == Condition)
               {
                   tempSubKey = n.Kind.ToString();
                   tempSubKey = GetSubSymbolAndFindKey(tempSubKey);
                   if (tempSubKey != null && propertiesProfileScheme.ContainsKey(tempSubKey))
                   {
                       tempCount =propertiesProfileScheme[tempSubKey];
                       propertiesProfileScheme[tempSubKey] = ++tempCount;
                   }
               }
           });
            PrintProfile(propertiesProfileScheme);
        }
        public static void PrintProfile(Dictionary<string,int> profile)
        {
            foreach (var prop in profile)
            {
                Console.WriteLine("{0} Counter: {1}", prop.Key, prop.Value);
            }
        }
        static void Main(string[] args)
        {
            var demoQuery = "DynamicOE| " +
                "where env_time > ago(7d)| where operationName == 'WriteTelemetryEvent'| extend userinfo1 = parse_json(customData)" +
                "| extend data = customData.data| extend dim = data.context| extend HostingBladeName = tostring(dim.HostingBladeName)|" +
                " where HostingBladeName == 'SecurityGraphBlade' or HostingBladeName startswith strcat('SecurityGraphBlade', '.')" +
                "| extend EventName = tostring(dim.name)| where EventName == 'RunCloudMapQuery'| where dim.Source startswith 'DataProvider.Fetch'" +
                "| where dim.Success == true | where dim.Skip == 0 and isempty(dim.AdditionalQuery) " +
                "| extend inputQuery = dim.BaseQuery, resultCount = dim.TotalRecords, durationMs = dim.durationMs, corelation = tostring(dim.ExecutionId), userAlias = tostring(dim.userAlias)" +
                "| project inputQuery, userAlias, corelation| join(FabricServiceOE)| project env_time, QueryRunDurationMs, ScopesCount, QueryResponseCount, QueryResponseTotalRecords," +
                " Query, Scopes, QueryLookupCount, QueryJoinCount, corelation = tostring(dim['CorrelationId'])) on corelation";
            GetProfile(demoQuery);
        }
    }
}
