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
using QueryProfiler.Profile;
using QueryProfiler.Optimization;
namespace QueryProfiler
{
    class Program
    {
        static void Main(string[] args)
        {
            var query = "Table1 | join (Table2) on CommonColumn, $left.Col1 == $right.Col2 ";
            var parseCode = KustoCode.Parse(query);
           ProfileAnalyzer.GetProfile(query);
           OptimalProposalForQuery.GetListOfPropsalToQuery(parseCode);
           XmlOptimalProposals.GetProposalsOptimization();
        }
    }
}