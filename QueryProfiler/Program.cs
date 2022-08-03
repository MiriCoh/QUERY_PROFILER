using Kusto.Language;
using Kusto.Language.Syntax;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Profile
{
    class Program
    {
        public static string GetSubSymbolAndFindKey(string strToSub, out string kindsParent)
        {
            Console.WriteLine("to add changes to pull req...");
            string[] symbolArray = { "Operator", "Condition", "Expression" };
            foreach (var symbol in symbolArray)
            {
                if (strToSub.Contains(symbol))
                {
                    kindsParent = symbol;
                    return strToSub.Substring(0, strToSub.Length - symbol.Length);
                }
            }
            kindsParent = null;
            return null;
        }
        public static void GetProfile(string query)
        {
            KustoCode code = KustoCode.Parse(query);
            Hashtable profile = new Hashtable();
            Hashtable tempHt = new Hashtable();
            string kindsParent = null, tempSubKey = null, expression = "Expression", condition = "Condition", Operator = "Operator";
            int tempCount = 0;
            SyntaxElement.WalkNodes(code.Syntax,
           n =>
           {
               if (n.NameInParent == Operator || n.NameInParent == condition || n.NameInParent == expression)
               {

                   tempSubKey = n.Kind.ToString();
                   tempSubKey = GetSubSymbolAndFindKey(tempSubKey, out kindsParent);
                   if (tempSubKey == null)
                   {
                       if (n.Kind.ToString() == "NameReference" && n.NameInParent == expression)
                       {
                           kindsParent = "DataTable";
                           if (!profile.ContainsKey(kindsParent))
                               profile.Add(kindsParent, new Hashtable());
                           tempHt = (Hashtable)profile[kindsParent];
                           if (!tempHt.ContainsKey(n))
                               tempHt.Add(n, 1);
                           else
                           {
                               tempCount = (int)tempHt[n];
                               tempHt[n] = ++tempCount;
                           }
                       }
                   }
                   else
                   {
                       if (!profile.ContainsKey(kindsParent) || profile.Count == 0)
                       {
                           profile.Add(kindsParent, new Hashtable());
                           tempHt = (Hashtable)profile[kindsParent];
                           tempHt.Add(tempSubKey, 1);
                       }
                       else
                       {
                           tempHt = (Hashtable)profile[kindsParent];
                           if (!tempHt.ContainsKey(tempSubKey))
                           {
                               tempHt.Add(tempSubKey, 1);
                           }
                           else
                           {
                               tempCount = (int)tempHt[tempSubKey];
                               tempHt[tempSubKey] = ++tempCount;
                           }
                       }
                   }
                   if (kindsParent != null)
                       profile[kindsParent] = tempHt;
               }
           });
            ConvertProfileToJsonAndPrint(profile);
        }
        public static void ConvertProfileToJsonAndPrint(Hashtable profile)
        {
            string s = JsonConvert.SerializeObject(new { Profile = profile });
            PrintJsonProfile(JObject.Parse(s));

        }
        public static void PrintJsonProfile(JObject parsedProfile)
        {
            foreach (var pair in parsedProfile)
            {
                Console.WriteLine("{0}: {1}", pair.Key, pair.Value);
            }
        }
        static void Main(string[] args)
        {
            var demoQuery = "T | project a = a + b | where a  > 10.0 | search a or search a";
            GetProfile(demoQuery);
        }
    }
}
