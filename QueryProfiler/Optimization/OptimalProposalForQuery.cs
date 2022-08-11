using Kusto.Language;
using Kusto.Language.Syntax;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
namespace QueryProfiler.Optimization
{
    class OptimalProposalForQuery:OptimalProposals
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
                   case "ContainsCsExpression":
                   case "HasExpression":
                       proposalOptimizations.AddRange(OperatorTranslator(Operator.Kind, Operator.NameInParent));
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
                       proposalOptimizations.AddRange(OperatorTranslator(Operator.Kind,Operator.NameInParent));
                       break;
                   default:
                       break;
               }
           });
            PrintListOfPropsalToQuery(proposalOptimizations);
            return proposalOptimizations;
        }
        private static List<ProposalScheme> OperatorTranslator(SyntaxKind operat, string kind)
        {
            var subKindFromOperatorName = operat.ToString().Substring(0, operat.ToString().Length - kind.Length);
            var Result= proposalsOptimization.FindAll(x => x.sourceOperator == subKindFromOperatorName);
            return Result;
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
