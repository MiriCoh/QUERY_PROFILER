using Kusto.Language;
using Kusto.Language.Syntax;
using System;
using System.Reflection;

namespace QueryProfiler.Profile
{
    public class ProfileAnalyzer
    {
        public static void GetProfile(string query)
        {
            var profileScheme = new ProfileScheme();
            var code = KustoCode.Parse(query);
            SyntaxElement.WalkNodes(code.Syntax,
           Operator =>
           {
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
        }
        private static ProfileScheme OperatorTranslator(ProfileScheme profileScheme,SyntaxKind operat,string kind)
        {
            var propertyName = GetSubSKind(operat.ToString(),kind);
            var pinfo = typeof(ProfileScheme).GetProperty(propertyName + "Counter");
            var value = (int)pinfo.GetValue(profileScheme);
            pinfo.SetValue(profileScheme, value + 1);
            return profileScheme;
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
                Console.WriteLine("[ " + p.Name + " " + p.GetValue(profile) + " ]");
            }
        }
    }

}
