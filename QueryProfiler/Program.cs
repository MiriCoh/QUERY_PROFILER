using Kusto.Language;
using Kusto.Language.Symbols;
using Kusto.Language.Syntax;
using Kusto.Language.Parsing;
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
            var a = "Table1 | join (Table2) on CommonColumn, $left.Col1 == $right.Col2";
            var c = KustoCode.Parse(a);
            //ProfileAnalyzer.GetProfile(a);
            //OptimalProposalForQuery.InitializingDatabaseOptimalOperators();
            //OptimalProposalForQuery.GetListOfPropsalToQuery(c);
            //GetDatabaseTables1(c);

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
            //GetDatabaseTables2(demoQuery);
            //ProfileAnalyzer.GetProfile("datatable (a:int, b:dynamic, c:dynamic)[1,dynamic({'prop1':'a', 'prop2':'b'}), dynamic([5, 4, 3])]| mv-expand b, c");
            //DbProposal.InitializingDatabaseOptimalOperators();
            // OptimalProposalForQuery.GetListOfPropsalToQuery(demoQuery);

            //GetDatabaseTableColumns(code);
            var globals = GlobalState.Default.WithDatabase(
             new DatabaseSymbol("db",
                 new TableSymbol("Shapes", "(id: string, width: real, height: real)")//,
               //  new FunctionSymbol("TallShapes", "{ Shapes | where width < height; }")
                 ));
            var query = "Table1 | join (Table2) on CommonColumn, $left.Col1 == $right.Col2";
            var cod = KustoCode.Parse(query);
            var q = cod.Analyze();
            Console.WriteLine(q.ToString());
            var code = KustoCode.ParseAndAnalyze(query, globals);
            var zz = code.ResultType.Name;
            var xx = code.GetSymbolsInScope(2,SymbolMatch.Table,IncludeFunctionKind.All);
            var x = cod.GetLexicalTokens();//פונקציה בשביל ריקי !!!
            int aaaa = 0, b = 0;
            var y = cod.TryGetLineAndOffset(2, out aaaa, out b);
            var parseCode = cod.Analyze();
            var z = parseCode.ResultType.Members;
            Console.WriteLine("***"+z.ToString());
            var e = z[0];
            var globals2 = GlobalState.Default.WithDatabase(
          new DatabaseSymbol("db",
              new TableSymbol(parseCode.ResultType.Display)//,
                                                                                  //  new FunctionSymbol("TallShapes", "{ Shapes | where width < height; }")
              ));

            var query_1 = "Table1 | project a ,b,c";
            code = KustoCode.Parse(query);
            var parseCode1 = code.Analyze();
            Console.WriteLine("parse code "+parseCode.ToString());
            Console.WriteLine("Columns - " + parseCode1.ResultType.Display);
            GetDatabaseTableColumns2(parseCode1);
            var code11 = KustoCode.ParseAndAnalyze(query, globals2);
            GetDatabaseTables2(code11);
            Console.WriteLine("parseCode - - - "+parseCode.ResultType.Display.ToString());
            var ty = parseCode.ResultType.Members.ToString();
            var cc = parseCode.Globals.GetTable((ColumnSymbol)e);
            Console.WriteLine("parseCode - "+cc);
            GetDatabaseTableColumns1(code11);
            var kusto = string.Format("let MyData = CompanyMydata" +
                " | where ID == 'Z123' | top 1 by dateTimeUtc desc");
            var xw = cod.GetLexicalTokens();
            var r = KustoCode.ParseAndAnalyze(query);
            Console.WriteLine(r.ResultType.Display.ToString());
            GetDatabaseTableColumns1(r);
            var ll = c.ResultType;//.Display.ToString());//GetTable.Description.ToString());
            Console.WriteLine("members "+ parseCode.ResultType.Name.ToString());
           // Console.WriteLine(parseCode.GetTokenIndex(5));
            //KustoCode code = KustoCode.Parse(query);
            var a1 = GetDatabaseTables1(cod);
            Console.WriteLine(a1);
            //var a2 = GetDatabaseTableColumns2(code);
            var globals1 = GlobalState.Default;
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
            var tables = new HashSet<TableSymbol>();
            var columns = new HashSet<ColumnSymbol>();
            GatherColumns(code.Syntax);
            foreach (var item in columns)
            {
                Console.WriteLine(item.Name);
            }
            foreach (var item in tables)
            {
                Console.WriteLine("name"+item);
            }
            Console.WriteLine("GetDatabaseTableColumns2");
            Console.WriteLine(" ");
            return columns;

            void GatherColumns(SyntaxNode root)
            {
                SyntaxElement.WalkNodes(root,
                    fnBefore: n =>
                    {
                        if (n.ReferencedSymbol is ColumnSymbol c)
                          //  && code.Globals.GetTable(c) != null)
                        {
                            tables.Add(code.Globals.GetTable(c));
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
                    if (n.Kind.ToString() == "NameReference" && n.NameInParent == "Expression")
                    {
                        Console.WriteLine(n);
                    }
                    if (n.GetTokens().ToString()=="table1"||n.ToString()=="table2")
                        Console.WriteLine(n.ToString());
                    // if (n.ReferencedSymbol is TableSymbol t
                    //    && code.Globals.IsDatabaseTable(t))
                    //{
                    //    tables.Add(t);
                    //}
                   //if (n is Expression && n.ReferencedSymbol is TableSymbol)
                       if( n is TableSymbol )
                        //&& code.Globals.IsDatabaseTable(ts))
                    {
                       // Console.WriteLine(n); //tables.Add(n);
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
