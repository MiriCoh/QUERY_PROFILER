using Kusto.Language;
using Kusto.Language.Symbols;
using Kusto.Language.Syntax;
using Kusto.QueryLanguage;
using Kusto.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using Kusto.Cloud.Platform.Data;
using Kusto.Data.Data;
using Kusto.Data.Net.Client;
using Kusto.Language.Editor;
using Kusto.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using QueryProfiler.Profile;
using QueryProfiler.Optimization;

namespace QueryProfiler
{
    class Program
    {
        static void Main(string[] args)
        {
            //var demoQuery = "DynamicOE| " +
            //    "where env_time > ago(7d)| where operationName == 'WriteTelemetryEvent'| extend userinfo1 = parse_json(customData)" +
            //    "| extend data = customData.data| extend dim = data.context| extend HostingBladeName = tostring(dim.HostingBladeName)|" +
            //    " where HostingBladeName == 'SecurityGraphBlade' or HostingBladeName startswith strcat('SecurityGraphBlade', '.')" +
            //    "| extend EventName = tostring(dim.name)| where EventName == 'RunCloudMapQuery'| where dim.Source startswith 'DataProvider.Fetch'" +
            //    "| where dim.Success == true | where dim.Skip == 0 and isempty(dim.AdditionalQuery) " +
            //    "| extend inputQuery = dim.BaseQuery, resultCount = dim.TotalRecords, durationMs = dim.durationMs, corelation = tostring(dim.ExecutionId), userAlias = tostring(dim.userAlias)" +
            //    "| project inputQuery, userAlias, corelation| join(FabricServiceOE)| project env_time, QueryRunDurationMs, ScopesCount, QueryResponseCount, QueryResponseTotalRecords," +
            //    " Query, Scopes, QueryLookupCount, QueryJoinCount, corelation = tostring(dim['CorrelationId'])) on corelation";
            KustoCode demoQuery = KustoCode.Parse("Table1 | join (Table2) on CommonColumn, $left.Col1 == $right.Col2");
          aaa(demoQuery);
            GetDatabaseTables2(demoQuery);
            
            ProfileAnalyzer.GetProfile("datatable (a:int, b:dynamic, c:dynamic)[1,dynamic({'prop1':'a', 'prop2':'b'}), dynamic([5, 4, 3])]| mv-expand b, c");
            DbProposal.InitializingDatabaseOptimalOperators();
             OptimalProposalForQuery.GetListOfPropsalToQuery(demoQuery);
       
            //GetDatabaseTableColumns(code);
            var globals = GlobalState.Default.WithDatabase(
             new DatabaseSymbol("db",
                 new TableSymbol("Shapes", "(id: string, width: real, height: real)")//,
               //  new FunctionSymbol("TallShapes", "{ Shapes | where width < height; }")
                 ));
            var query = "Shapes | where width > 5 | project id, width";
             var code = KustoCode.ParseAndAnalyze(query, globals);
            //KustoCode code = KustoCode.Parse(query);
             var a1 = GetDatabaseTables2(code);
            Console.WriteLine(a1);
            //var a2 = GetDatabaseTableColumns2(code);
            var globals1 = GlobalState.Default;
        }
        public static string GetSubSymbolAndFindKey(string strToSub, out string kindsParent)
        {if (strToSub.Contains("Operator") || strToSub.Contains("Condition") || strToSub.Contains("Expression"))
            {
                string[] symbolArr = { "Operator", "Condition", "Expression" };
                foreach (var symbol in symbolArr)
                {
                    if (strToSub.Contains(symbol))
                    {
                        kindsParent = symbol;
                        return strToSub.Substring(0, strToSub.Length - symbol.Length);
                    }
                }
            }
            kindsParent = null;
            return null;
        }
        public static void aaa(KustoCode code)
        {
            var prop = new ProfileScheme();
            List<string> profile = new List<string>();
            int tempCount = 0;
            string aa = " ";
            string kindsParent = null;
            SyntaxElement.WalkNodes(code.Syntax,
                n =>
                {
                    var aaa = GetSubSymbolAndFindKey(n.Kind.ToString(), out aa);
                    if (aaa != null)
                    {
                        PropertyInfo pinfo = typeof(ProfileScheme).GetProperty(aaa + "Counter");
                        if (pinfo != null)
                        {
                            var value = pinfo.GetValue(prop);

                            pinfo.SetValue(prop, 1);
                        }
                    }
                    var tempSubKey = n.Kind.ToString();
                    tempSubKey = GetSubSymbolAndFindKey(tempSubKey, out kindsParent);
                    if (n.Kind.ToString() == "NameReference" && n.NameInParent == "Expression")
                    {
                        profile.Add(n.ToString());
                    }
                });
            Console.WriteLine(prop);
            foreach (var item in profile)
            {
                Console.WriteLine(item);
            }
            var t = typeof(ProfileScheme);
            foreach (PropertyInfo p in t.GetProperties())
            {
                Console.WriteLine("[ " + p + " " +p.GetValue(prop)+ " ]");
            }
        }

        public static HashSet<ColumnSymbol> GetDatabaseTableColumns1(KustoCode code)
        {
            var columns = new HashSet<ColumnSymbol>();

            SyntaxElement.WalkNodes(code.Syntax,
                n =>
                {
                    if (n.ReferencedSymbol is ColumnSymbol c
                        && code.Globals.GetTable(c) != null)
                    {
                        columns.Add(c);
                    }
                });
            foreach (var item in columns)
            {
                Console.WriteLine(item.Name);
            }
            Console.WriteLine(" ");
            Console.WriteLine("GetDatabaseTableColumns1");
            Console.WriteLine(" ");
            return columns;
        }
        public static HashSet<ColumnSymbol> GetDatabaseTableColumns2(KustoCode code)
        {
            var columns = new HashSet<ColumnSymbol>();
            GatherColumns(code.Syntax);
            foreach (var item in columns)
            {
                Console.WriteLine(item.Name);
            }
            Console.WriteLine("GetDatabaseTableColumns2");
            Console.WriteLine(" ");
            return columns;

            void GatherColumns(SyntaxNode root)
            {
                SyntaxElement.WalkNodes(root,
                    fnBefore: n =>
                    {
                        if (n.ReferencedSymbol is ColumnSymbol c
                            && code.Globals.GetTable(c) != null)
                        {
                            columns.Add(c);
                        }
                        else if (n.GetCalledFunctionBody() is SyntaxNode body)
                        {
                            GatherColumns(body);
                        }
                    },
                    fnDescend: n =>
                        // skip descending into function declarations since their bodies will be examined by the code above
                        !(n is FunctionDeclaration)
                    );
            }
        }
        public static HashSet<TableSymbol> GetDatabaseTables1(KustoCode code)
        {
            var tables = new HashSet<TableSymbol>();

            SyntaxElement.WalkNodes(code.Syntax,
                n =>
                {
                    if (n.ReferencedSymbol is TableSymbol t
                        && code.Globals.IsDatabaseTable(t))
                    {
                        tables.Add(t);
                    }
                    else if (n is Expression e
                        && e.ResultType is TableSymbol ts
                        && code.Globals.IsDatabaseTable(ts))
                    {
                        tables.Add(ts);
                    }
                });
            foreach (var item in tables)
            {
                Console.WriteLine(item.Name);
            }
            Console.WriteLine("GetDatabaseTables1");
            Console.WriteLine(" ");
            return tables;
        }
        public static HashSet<TableSymbol> GetDatabaseTables2(KustoCode code)
        {
            var tables = new HashSet<TableSymbol>();
            GatherTables(code.Syntax);
            foreach (var item in tables)
            {
                Console.WriteLine(item.Name);
            }
            Console.WriteLine(" ");
            Console.WriteLine("GetDatabaseTables2");
            return tables;

            void GatherTables(SyntaxNode root)
            {
                SyntaxElement.WalkNodes(root,
                    fnBefore: n =>
                    {
                        if (n.ReferencedSymbol is TableSymbol t
                            && code.Globals.IsDatabaseTable(t))
                        {
                            tables.Add(t);
                        }
                        else if (n is Expression e
                            && e.ResultType is TableSymbol ts
                            && code.Globals.IsDatabaseTable(ts))
                        {
                            tables.Add(ts);
                        }
                        else if (n.GetCalledFunctionBody() is SyntaxNode body)
                        {
                            GatherTables(body);
                        }
                    },
                    fnDescend: n =>
                        // skip descending into function declarations since their bodies will be examined by the code above
                        !(n is FunctionDeclaration)
                    );
            }
        }
    }
}
