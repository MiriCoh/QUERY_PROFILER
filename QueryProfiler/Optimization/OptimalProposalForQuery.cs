﻿using Kusto.Language;
using Kusto.Language.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QueryProfiler.Optimization
{
    class OptimalProposalForQuery
    {
        public static List<ProposalScheme> GetListOfPropsalToQuery(KustoCode code)
        {
            var proposalsOptimization = new List<ProposalScheme>();
            SyntaxElement.WalkNodes(code.Syntax,
           Operator =>
           {
               switch (Operator.Kind.ToString())
               {
                   case "ContainsExpression":
                   case "ContainsCsExpression":
                   case "HasExpression":
                       proposalsOptimization.AddRange(OperatorTranslator(Operator.Kind, Operator.NameInParent));
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
                       proposalsOptimization.AddRange(OperatorTranslator(Operator.Kind,Operator.NameInParent));
                       proposalsOptimization.ToList().ForEach(c => c.OperatorPosition = Operator.TextStart);
                       break;
                   default:
                       break;
               }
           });
            PrintListOfPropsalToQuery(proposalsOptimization);
            foreach (var prop in proposalsOptimization)
            {
               // prop.ProposalPosition
            }
            return proposalsOptimization;
        }
        private static List<ProposalScheme> OperatorTranslator(SyntaxKind operat, string kind)
        {
            var propsals = new List<ProposalScheme>();
            var subKindFromOperatorName = operat.ToString().Substring(0, operat.ToString().Length - kind.Length);
            var proposalsOptimization =  XmlOptimalProposals.GetProposalsOptimization().ProposalsOptimization;
            var result=proposalsOptimization.FindAll(x => x.SourceOperator == subKindFromOperatorName);
            return result;
        }
        private static void PrintListOfPropsalToQuery(List<ProposalScheme> proposals)
        {
            foreach (var proposal in proposals)
            {
                Console.WriteLine("\n sourceOperator :" + proposal.SourceOperator
                                + "\n ProposalOptimalOperator :" + proposal.ProposalOptimalOperator 
                                + "\n ProposalReason :" + proposal.ProposalReason);
            }
        }
    }
}