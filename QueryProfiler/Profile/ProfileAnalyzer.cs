using Kusto.Language;
using Kusto.Language.Symbols;
using Kusto.Language.Syntax;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Reflection;
using ICSharpCode.Decompiler.Util;
namespace QueryProfiler.Profile
{
    public class ProfileAnalyzer
    {
        public static ProfileScheme GetProfile(string query)
        {

            var profileScheme = new ProfileScheme();
            var code = KustoCode.ParseAndAnalyze(query);
            SyntaxElement.WalkNodes(code.Syntax,
           Operator =>
           {
               if (Operator is Expression e
               && e.RawResultType is TableSymbol
               && Operator.Kind.ToString() == "NameReference")
                   profileScheme.Tables.Add(e.ToString());
               switch (Operator)
               {
                   case InExpression t1:
                   case JoinOperator t2:
                   case LookupOperator t3:
                   case UnionOperator t4:
                   case MvExpandOperator t5:
                       profileScheme = OperatorTranslator(profileScheme, Operator.Kind, Operator.NameInParent);
                       break;
                   default:
                       break;
               }
           });
            return profileScheme;
        }
        private static ProfileScheme OperatorTranslator(ProfileScheme profileScheme, SyntaxKind operat, string kind)
        {
            var propertyName = GetSubKind(operat.ToString(), kind);
            var propertyInfo = typeof(ProfileScheme).GetProperty(propertyName + "Counter");
            var value = (int)propertyInfo.GetValue(profileScheme);
            propertyInfo.SetValue(profileScheme, value + 1);
            return profileScheme;
        }
        private static string GetSubKind(string strToSub, string kind)
        {
            return strToSub.Substring(0, strToSub.Length - kind.Length);
        }

    }

}
