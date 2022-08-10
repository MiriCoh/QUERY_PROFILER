using Kusto.Language;
using Kusto.Language.Syntax;
using System;
using System.Collections.Generic;
namespace QueryProfiler.Optimization
{
    class OptimalProposalForQuery:DbProposal
    {
        public static List<ProposalScheme> GetListOfPropsalToQuery(KustoCode code)
        {
            var proposalOptimizations = new List<ProposalScheme>();

            SyntaxElement.WalkNodes(code.Syntax,
           Operator =>
           {
               switch (Operator.Kind.ToString())
               {
                   case "ContainsExpression":
                       proposalOptimizations.Add(dbProposalOptimization[0]);
                       proposalOptimizations.Add(dbProposalOptimization[1]);
                       break;
                   case "ContainsCsExpression":
                       proposalOptimizations.Add(dbProposalOptimization[2]);
                       break;
                   case "HasExpression":
                       proposalOptimizations.Add(dbProposalOptimization[3]);
                       break;
                   default:
                       break;
               }
               switch (Operator)
               {
                   case HasAnyExpression t:
                       proposalOptimizations.Add(dbProposalOptimization[4]);
                       break;
                   case InExpression t:
                       proposalOptimizations.Add(dbProposalOptimization[5]);
                       break;
                   case LookupOperator t:
                       proposalOptimizations.Add(dbProposalOptimization[6]);
                       break;
                   case JoinOperator t:
                       proposalOptimizations.Add(dbProposalOptimization[7]);
                       proposalOptimizations.Add(dbProposalOptimization[8]);
                       proposalOptimizations.Add(dbProposalOptimization[9]);
                       break;
                   case SearchOperator t:
                       proposalOptimizations.Add(dbProposalOptimization[10]);
                       break;
                   default:
                       break;
               }
           });
            PrintListOfPropsalToQuery(proposalOptimizations);
            return proposalOptimizations;
        }
        private static void PrintListOfPropsalToQuery(List<ProposalScheme> proposals)
        {
            foreach (var proposal in proposals)
            {
                Console.WriteLine("\n sourceOperator :" + proposal.sourceOperator
                                + "\n ProposalOptimalOperator :" + proposal.ProposalOptimalOperator 
                                + "\n ProposalReason :" + proposal.ProposalReason);
            }
        }
    }
}
