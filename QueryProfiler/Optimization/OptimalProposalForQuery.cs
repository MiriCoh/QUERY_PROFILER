using Kusto.Cloud.Platform.Utils;
using Kusto.Language;
using Kusto.Language.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
namespace QueryProfiler.Optimization
{
   public class OptimalProposalForQuery
    {
        public static List<ProposalScheme> GetListOfPropsalToQuery(string query)
        {
            var code = KustoCode.Parse(query);
            var currentProposalsOptimization = new List<ProposalScheme>();
            SyntaxElement.WalkNodes(code.Syntax,
           Operator =>
           {
               switch (Operator.Kind.ToString())
               {
                   case "ContainsExpression":
                   case "ContainsCsExpression":
                   case "HasExpression":
                       currentProposalsOptimization = AddProposalsAndUpdatePosition(currentProposalsOptimization, Operator);
                       break;
                   default:
                       break;
               }
               switch (Operator)
               {
                   case HasAnyExpression t1:
                   case InExpression t2:
                   case LookupOperator t3:
                   case JoinOperator t4:
                   case SearchOperator t5:
                       currentProposalsOptimization = AddProposalsAndUpdatePosition(currentProposalsOptimization, Operator);
                       break;
                   default:
                       break;
               }
           });
            PrintAddProposalsAndUpdatePositionProposalsOptimizationOfPropsalToQuery(currentProposalsOptimization);
            var cpo = currentProposalsOptimization.Distinct();
            List<ProposalScheme> p = new List<ProposalScheme>();
            foreach (var item in cpo)
            {
                p.Add(item);
            }
                return p;
        }
        private static List<ProposalScheme> AddProposalsAndUpdatePosition(List<ProposalScheme> currentProposalsOptimization, SyntaxNode Operator)
        {
            List<ProposalScheme> proposals = OperatorTranslator(Operator.Kind, Operator.NameInParent);
            proposals.ForEach(op => op.OperatorPosition = Operator.TextStart);
            currentProposalsOptimization.SafeAddRange(proposals);
            return currentProposalsOptimization;
        }
        private static List<ProposalScheme> OperatorTranslator(SyntaxKind operat, string kind)
        {
            var subKindFromOperatorName = operat.ToString().Substring(0, operat.ToString().Length - kind.Length);
            var proposalsOptimization =  XmlOptimalProposals.GetProposalsOptimization().ProposalsOptimization;
            var result=proposalsOptimization.FindAll(proposal => proposal.SourceOperator == subKindFromOperatorName);
            return result;
        }
        private static void PrintAddProposalsAndUpdatePositionProposalsOptimizationOfPropsalToQuery(List<ProposalScheme> proposals)
        {
            foreach (var proposal in proposals)
            {
                Console.WriteLine("\n sourceOperator :" + proposal.SourceOperator
                                + "\n ProposalOptimalOperator :" + proposal.ProposalOptimalOperator 
                                + "\n ProposalReason :" + proposal.ProposalReason
                                 + "\n OperatorPosition :" + proposal.OperatorPosition);
            }
        }
    }
}