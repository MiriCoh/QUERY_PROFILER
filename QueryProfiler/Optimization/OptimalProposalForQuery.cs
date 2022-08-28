using Kusto.Cloud.Platform.Utils;
using Kusto.Language;
using Kusto.Language.Symbols;
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
            var currentKeywords = new Dictionary<SyntaxNode, int>();
            SyntaxElement.WalkNodes(code.Syntax,
           Operator =>
           {
               switch (Operator.Kind.ToString())
               {
                   case "ContainsExpression":
                   case "ContainsCsExpression":
                   case "HasExpression":
                       currentKeywords.Add(Operator, Operator.TextStart);
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
                       currentKeywords.Add(Operator, Operator.TextStart);
                       break;
                   default:
                       break;
               }
           });
            currentProposalsOptimization = AddProposalsAndUpdatePosition(currentKeywords);
            return currentProposalsOptimization;
        }
        private static List<ProposalScheme> AddProposalsAndUpdatePosition(Dictionary<SyntaxNode,int> currentProposalsOptimizations)
        {
            var proposals = XmlOptimalProposals.GetProposalsOptimization();
            var currentProposals = new List<ProposalScheme>();
            foreach (var keyword in currentProposalsOptimizations)
            {
                var operatorKind = GetOperatorKind(keyword.Key.Kind.ToString(), keyword.Key.NameInParent);
                var proposal= proposals.ProposalsOptimization.Find(op => op.SourceOperator == operatorKind);
                proposal.OperatorPosition = keyword.Value;
                currentProposals.Add(proposal);
            }
            return currentProposals;
        }
        private static string GetOperatorKind(string strToSub, string kind)
        {
            return strToSub.Substring(0, strToSub.Length - kind.Length);
        }
    }
}