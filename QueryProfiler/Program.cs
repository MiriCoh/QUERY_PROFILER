using Kusto.Language;
using QueryProfiler.Profile;
using QueryProfiler.Optimization;
namespace QueryProfiler
{
    class Program
    {
        static void Main(string[] args)
        {
          //var query = "Table1 | join (Table2) on CommonColumn, $left.Col1 == $right.Col2 ";
           var query = "R1 | join R2 on Region| join R3 on Region";
           ProfileAnalyzer.GetProfile(query);
           OptimalProposalForQuery.GetListOfPropsalToQuery(query);
        }
    }
}