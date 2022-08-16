using Kusto.Language;
using Kusto.Language.Symbols;
using Kusto.Language.Syntax;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace QueryProfiler.Profile
{
    public class ProfileAnalyzer
    {
        public static JObject GetProfile(string query)
        {
            var profileScheme = new ProfileScheme();
            var code = KustoCode.Parse(query).Analyze();
            SyntaxElement.WalkNodes(code.Syntax,
           Operator =>
           {
               if (Operator is Expression e && e.RawResultType is TableSymbol && Operator.Kind.ToString() == "NameReference")
                   profileScheme.Tables.Add(e.ToString());
               switch (Operator)
               {
                   case InExpression t1:
                   case JoinOperator t2:
                   case LookupOperator t3:
                   case UnionOperator t4:
                   case MvExpandOperator t5:
                       profileScheme = OperatorTranslator(profileScheme,Operator.Kind,Operator.NameInParent);
                       break;
                   default:
                       break;
               }
           });
            PrintProfile(profileScheme);
            return ConvertProfileToJson(profileScheme);
        }
        private static ProfileScheme OperatorTranslator(ProfileScheme profileScheme,SyntaxKind operat,string kind)
        {
            var propertyName = GetSubSKind(operat.ToString(),kind);
            var propertyInfo = typeof(ProfileScheme).GetProperty(propertyName + "Counter");
            var value = (int)propertyInfo.GetValue(profileScheme);
            propertyInfo.SetValue(profileScheme, value + 1);
            return profileScheme;
        }
        private static JObject ConvertProfileToJson(ProfileScheme profile)
        {
            var ConvertProfileToJson = JsonConvert.SerializeObject(new { Profile = profile });
            return JObject.Parse(ConvertProfileToJson); 
        }
        private static string GetSubSKind(string strToSub,string kind)
        {
           return strToSub.Substring(0, strToSub.Length - kind.Length);
        }
        private static void PrintProfile(ProfileScheme profile)
        {
            var t = typeof(ProfileScheme);
            foreach (PropertyInfo p in t.GetProperties())
            {
                Console.WriteLine("[ " + p.Name + " " + p.GetValue(profile).ToString() + " ]");
            }
        }
    }

}
