using Kusto.Language;
using QueryProfiler.Profile;
using QueryProfiler.Optimization;
namespace QueryProfiler
{
    class Program
    {
        static void Main(string[] args)
        {
            
           var query = "Table1 | join (Table2) on CommonColumn, $left.Col1 == $right.Col2 ";
           ProfileAnalyzer.GetProfile(query);
           OptimalProposalForQuery.GetListOfPropsalToQuery(query);
        }
    }
}
