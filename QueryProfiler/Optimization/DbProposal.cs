using Kusto.Language;
using Kusto.Language.Syntax;
using System.Collections.Generic;
namespace QueryProfiler
{
    class DbProposal
    {
        static List<ProposalScheme> dbProposalOptimization = new List<ProposalScheme>();
        private static void InitializingDatabaseOptimalOperators()
        {
            
            var contains_1 = new ProposalScheme
            {  
                // רגיש לאותיות גדולות לכן החיפוש יהיה ממוקד יותר ומהיר יותר cs ערך בסיומת
                sourceOperator = "contains",
                ProposalOptimalOperator = "contains_cs",
                ProposalReason  = "Use contains_cs instead of contains because the value with a cs suffix is ​​case sensitive so the search will be more focused and faster than contains"
            };
            var contains_2 = new ProposalScheme
            {  
                //עושה חיפוש לפי האינדקס של העמודות has
                //עושה חיפוש על מילים שלמות has
                //ולכן זה יעיל יותר
                sourceOperator = "contains",
                ProposalOptimalOperator = "has",
                ProposalReason  = "When looking for full tokens, has works better, since it doesn't look for substrings"
                //has does a search by the index of the columns
                //and does a search on whole words 
                //and therefore it is more efficient than contains

            };
            var contains_cs = new ProposalScheme
            {
                //עושה חיפוש לפי האינדקס של העמודות has
                //עושה חיפוש על מילים שלמות has
                //ולכן זה יעיל יותר
                sourceOperator = "contains_cs",
                ProposalOptimalOperator = "has_cs",
                ProposalReason  = "Use contains_cs instead of has_cs because has_cs does a search by the index of the columns and does a search on whole words and therefore it is more efficient than contains"
            };
            var has = new ProposalScheme
            {
                // רגיש לאותיות גדולות לכן החיפוש יהיה ממוקד יותר ומהיר יותר cs ערך בסיומת
                sourceOperator = "has",
                ProposalOptimalOperator = "has_cs",
                ProposalReason  = "Use has instead of has_cs because the value with the suffix cs is case sensitive so the search will be more focused and faster than has"
            };
            var has_any = new ProposalScheme
            {
                sourceOperator = "has_any",
                ProposalOptimalOperator = "has",
                ProposalReason  = "if one table is always smaller than the other, use it as the left (piped) side of the join"
            };
            var In = new ProposalScheme
            {   //רגיש לאותיות גדולות in~
                //ולכן זה יעיל יותר
                sourceOperator = "in~",
                ProposalOptimalOperator = "in",
                ProposalReason  = "Use in~ instead of in because in~ is case sensitive, so the search is more focused and faster"
            };
            var lookup = new ProposalScheme
            {  // יושמטו null שורות בטבלה השמאלית שאין להם התאמה בטבלה הימנית במקום להיות  
                sourceOperator = "lookup",
                ProposalOptimalOperator = "lookup kind=leftouter",
                ProposalReason  = "Use lookup kind=leftouter instead of lookup because kind=leftouter put Null rows in the left table that do not have a match in the right table will be omitted instead of being"
            };
            var join_1 = new ProposalScheme
            {    //
                sourceOperator = "join",
                ProposalOptimalOperator = "left join",
                ProposalReason  = "if one table is always smaller than the other, use it as the left (piped) side of the join"
            };
            var join_2 = new ProposalScheme
            {    //
                sourceOperator = "join",
                ProposalOptimalOperator = "right join",
                ProposalReason  = "use right table if one table is always smaller than the other, use it as the left (piped) side of the join"
            };
            var search = new ProposalScheme
            {    //
                sourceOperator = "search",
                ProposalOptimalOperator = "where",
                ProposalReason  = "Prefer to filter inside the search operator"
            };
            dbProposalOptimization.Add(contains_1);
            dbProposalOptimization.Add(contains_2);
            dbProposalOptimization.Add(contains_cs);
            dbProposalOptimization.Add(has);
            dbProposalOptimization.Add(has_any);
            dbProposalOptimization.Add(In);
            dbProposalOptimization.Add(lookup);
            dbProposalOptimization.Add(join_1);
            dbProposalOptimization.Add(join_2);
            dbProposalOptimization.Add(search);
        }
        private static void GetListOfPropsalToQuery(KustoCode code)
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
                   default:
                       break;
               }
               switch (Operator)
               {
                   case HasAnyExpression t:
                       proposalOptimizations.Add(dbProposalOptimization[5]);
                       break;
                   //case InExpression t:
                   //    proposalOptimizations.Add(dbProposalOptimization[5]);
                   //    break;
                   case InExpression t:
                       proposalOptimizations.Add(dbProposalOptimization[5]);
                       break;
                   case LookupOperator t:
                       proposalOptimizations.Add(dbProposalOptimization[6]);
                       break;
                   case JoinOperator t:
                       proposalOptimizations.Add(dbProposalOptimization[7]);
                       proposalOptimizations.Add(dbProposalOptimization[8]);
                       break;
                
                   default:
                       break;

               }
           });

        }
    }
}