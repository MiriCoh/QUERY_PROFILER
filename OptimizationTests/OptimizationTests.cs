using Microsoft.VisualStudio.TestTools.UnitTesting;
using QueryProfiler;
using QueryProfiler.Optimization;
using System.Collections.Generic;
using System.Linq;
namespace OptimizationTests
{
    [TestClass]
    public class OptimizationTests
    {
        [TestMethod]
        public void TestGetSinglePropsalToQuery()
        {
          var query = "Table1 | join (Table2) on CommonColumn, $left.Col1 == $right.Col2 ";
            var actual = OptimalProposalForQuery.GetListOfPropsalToQuery(query);
            var expected = new List<ProposalScheme> {
                new ProposalScheme{
                SourceOperator="Join",
                ProposalOptimalOperator="lookup",
                ProposalReason="Use lookup instead of join because join needs more memory so lookup will be faster",
                OperatorPosition=9
                }
            };
            Assert.IsTrue(expected.SequenceEqual(actual, new ProposalSchemaEqualityComparer()));
        }
        [TestMethod]
        public void TestGetListOfPropsalToQuery()
        {
            var query = "R1 | join R2 on Region| join R3 on Region";
            var actual = OptimalProposalForQuery.GetListOfPropsalToQuery(query);
            var expected = new List<ProposalScheme> {
                new ProposalScheme{
                SourceOperator="Join",
                ProposalOptimalOperator="lookup",
                ProposalReason="Use lookup instead of join because join needs more memory so lookup will be faster",
                OperatorPosition=5
                },
                 new ProposalScheme{
                SourceOperator="Join",
                ProposalOptimalOperator="lookup",
                ProposalReason="Use lookup instead of join because join needs more memory so lookup will be faster",
                OperatorPosition=24
                }
            };
            Assert.IsTrue(expected.SequenceEqual(actual, new ProposalSchemaEqualityComparer()));
        }
    }
}