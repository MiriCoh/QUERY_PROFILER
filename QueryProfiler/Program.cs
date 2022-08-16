using Kusto.Language;
using Kusto.Language.Symbols;
using Kusto.Language.Syntax;
using Kusto.Ingest;
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
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.IO;

namespace QueryProfiler
{
    class Program
    {
        static void Main(string[] args)
        {
            var query = "Table1 | join (Table2) on CommonColumn, $left.Col1 == $right.Col2 ";
            var parseCode = KustoCode.Parse(query);
            ProfileAnalyzer.GetProfile(query);
           var result= OptimalProposalForQuery.GetListOfPropsalToQuery(parseCode);
            Console.WriteLine("config"+XmlOptimalProposals.GetProposalsOptimization());
        }
    }
}