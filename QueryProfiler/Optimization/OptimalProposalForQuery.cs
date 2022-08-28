using Kusto.Cloud.Platform.Utils;
using Kusto.Language;
using Kusto.Language.Syntax;
using System.Collections.Generic;
namespace QueryProfiler.Optimization
{
    public class OptimalProposalForQuery
    {
        public static List<ProposalScheme> GetListOfPropsalToQuery(string query)
        {
            if (!query.IsNotNullOrEmpty())
                return new List<ProposalScheme>();
            var code = KustoCode.Parse(query);
            var currentProposalsOptimization = new List<ProposalScheme>();
            var currentKeywords = new Dictionary<string, int>();
            SyntaxElement.WalkNodes(code.Syntax,
           Operator =>
           {
               switch (Operator.Kind.ToString())
               {
                   case "ContainsExpression":
                   case "ContainsCsExpression":
                   case "HasExpression":
                       currentKeywords.Add(Operator.ToString(), Operator.TextStart);
                       break;
                   default:
                       break;
               }
               switch (Operator)
               {
                   case HasAnyExpression operator1:
                   case InExpression operator2:
                   case LookupOperator operator3:
                   case JoinOperator operator4:
                   case SearchOperator operator5:
                       currentKeywords.Add(Operator.ToString(), Operator.TextStart);
                       break;
                   default:
                       break;
               }
           });
            currentProposalsOptimization = AddProposalsAndUpdatePosition(currentKeywords);
            return currentProposalsOptimization;
        }
        private static List<ProposalScheme> AddProposalsAndUpdatePosition(Dictionary<string,int> currentProposalsOptimizations)
        {
            var proposals = new List<ProposalScheme>();
            foreach (var keyword in currentProposalsOptimizations)
            {
                var proposal= proposals.Find(op => op.SourceOperator == keyword.Key);
                proposal.OperatorPosition = keyword.Value;
                proposals.Add(proposal);
            }
            return proposals;
        }

    }
}