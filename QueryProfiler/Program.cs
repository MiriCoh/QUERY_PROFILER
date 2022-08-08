using Kusto.Language;
using Kusto.Language.Syntax;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace QueryProfiler
{
    class Program
    {
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
        private static void GetProfile(string query)
        {
            var profileScheme = new ProfileScheme();
            var code = KustoCode.Parse(query);
            SyntaxElement.WalkNodes(code.Syntax,
           Operator =>
           {
               switch (Operator.Kind.ToString())
               {
                   case "ContainsExpression":
                       profileScheme.ContainsCounter += 1;
                       break;
                   case "ContainsCsExpression":
                       profileScheme.Contains_csCounter += 1;
                       break;
                   case "HasExpression":
                       profileScheme.HasCounter += 1;
                       break;
                   case "HasCsExpression":
                       profileScheme.Has_csCounter += 1;
                       break;
                   default:
                       break;
               }
           });
            PrintProfile(profileScheme);
        }
        private static void PrintProfile(ProfileScheme profile)
        {
            foreach (var p in profile.GetType().GetProperties())
            {
                Console.WriteLine("["+p.Name + " : " + p.GetValue(profile)+"]");
            }
        }
      
    }
}
