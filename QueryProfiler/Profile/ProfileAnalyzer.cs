using Kusto.Language;
using Kusto.Language.Syntax;
using System;
using System.Reflection;

namespace QueryProfiler.Profile
{
  public class ProfileAnalyzer
    {
        public static ProfileScheme profileScheme = new ProfileScheme();

        public static void GetProfile(string query)
        {
            var code = KustoCode.Parse(query);
            SyntaxElement.WalkNodes(code.Syntax,
           Operator =>
           {
              
               switch (Operator)
               {
                   case InExpression t:
                       profileScheme.InCounter += 1;
                       break;
                   case JoinOperator t:
                       profileScheme.JoinCounter += 1;
                       break;
                   case LookupOperator t:
                       profileScheme.LookupCounter += 1;
                       break;
                   case UnionOperator t:
                       profileScheme.UnionCounter += 1;
                       break;
                   case MvExpandOperator t:
                       profileScheme.MvExpandCounter += 1;
                       break;//mv-expand
                   default:
                       break;
               }
           });
            PrintProfile(profileScheme);
        }
        private static void OperatorTranslator(SyntaxNode operat)
        {
            var proppertyName = GetSubSymbol(operat.ToString());
            if (proppertyName != null)
            {
                PropertyInfo pinfo = typeof(ProfileScheme).GetProperty(proppertyName + "Counter");
                if (pinfo != null)
                {
                    var value = (int)pinfo.GetValue(profileScheme);
                    pinfo.SetValue(profileScheme, value++);
                }
            }
        }
        private static string GetSubSymbol(string strToSub)
        {
            if (strToSub.Contains("Operator") || strToSub.Contains("Condition") || strToSub.Contains("Expression"))
            {
                string[] symbolArr = { "Operator", "Condition", "Expression" };
                foreach (var symbol in symbolArr)
                {
                    if (strToSub.Contains(symbol))
                    {
                        return strToSub.Substring(0, strToSub.Length - symbol.Length);
                    }
                }
            }
            return null;
        }
        private static void PrintProfile(ProfileScheme profile)
        {
            var t = typeof(ProfileScheme);
            foreach (PropertyInfo p in t.GetProperties())
            {
                Console.WriteLine("[ "+p.Name+" "+p.GetValue(profile)+" ]");
            }
        }

    }
   
}
