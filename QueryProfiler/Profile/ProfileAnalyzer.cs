using Kusto.Cloud.Platform.Utils;
using Kusto.Language;
using Kusto.Language.Symbols;
using Kusto.Language.Syntax;
using System.Collections.Generic;
namespace QueryProfiler.Profile
{
    public class ProfileAnalyzer
    {
        public static ProfileScheme GetProfile(string query)
        {
            if (!query.IsNotNullOrEmpty()) return new ProfileScheme();
            var operators = new Dictionary<SyntaxKind, string>();
            var profileScheme = new ProfileScheme();
            var code = KustoCode.ParseAndAnalyze(query);
            SyntaxElement.WalkNodes(code.Syntax,
           Operator =>
           {
               if (Operator is Expression table
               && table.RawResultType is TableSymbol
               && Operator.Kind.ToString() == "NameReference")
                   profileScheme.Tables.Add(table.ToString());
               switch (Operator)
               {
                   case InExpression operator1:
                   case JoinOperator operator2:
                   case LookupOperator operator3:
                   case UnionOperator operator4:
                   case MvExpandOperator operator5:
                       operators.Add(Operator.Kind, Operator.NameInParent);
                       break;
                   default:
                       break;
               }
           });
            profileScheme = OperatorTranslator(profileScheme, operators);
            profileScheme = AddComplexityLevel(profileScheme);
            return profileScheme;
        }
        private static ProfileScheme AddComplexityLevel(ProfileScheme profileScheme)
        {
            var result = XmlComplexityLevel.GetComplexityLevel();
            int sumJoinUnionLookup = profileScheme.JoinCounter + profileScheme.LookupCounter + profileScheme.UnionCounter;
            var level = result.complexitiesLevel.Find(x => x.JoinUnionLookupCounter.range.from <= sumJoinUnionLookup && x.JoinUnionLookupCounter.range.to >= sumJoinUnionLookup);
            profileScheme.complexityLevel = level.Level;
            return profileScheme;
        }
        private static ProfileScheme OperatorTranslator(ProfileScheme profileScheme, Dictionary<SyntaxKind, string> operators)
        {
            foreach (var keyword in operators)
            {
                var propertyName = GetSubKind(keyword.Key.ToString(), keyword.Value);
                var propertyInfo = typeof(ProfileScheme).GetProperty(propertyName + "Counter");
                var value = (int)propertyInfo.GetValue(profileScheme);
                propertyInfo.SetValue(profileScheme, value + 1);
            }
            return profileScheme;
        }
        private static string GetSubKind(string strToSub, string kind)
        {
            return strToSub.Substring(0, strToSub.Length - kind.Length);
        }

    }

}
