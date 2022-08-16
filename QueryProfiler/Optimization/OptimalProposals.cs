using System.Collections.Generic;
namespace QueryProfiler
{
    class OptimalProposals
    {
       public static List<ProposalScheme> proposalsOptimization = new List<ProposalScheme>();
        public static void InitializingDatabaseOptimalOperators()
        {
            proposalsOptimization = new List<ProposalScheme>
            {
            new ProposalScheme
            {    // רגיש לאותיות גדולות לכן החיפוש יהיה ממוקד יותר ומהיר יותר cs ערך בסיומת
                sourceOperator = "Contains",
                ProposalOptimalOperator = "contains_cs",
                ProposalReason  = "Usecontains_cs instead of contains because the value with a cs suffix is ​​case sensitive so the search will be more focused and faster than contains"
            },
            new ProposalScheme
            {
                 //עושה חיפוש לפי האינדקס של העמודות has
                //עושה חיפוש על מילים שלמות has
                //ולכן זה יעיל יותר
                sourceOperator = "Contains",
                ProposalOptimalOperator = "has",
                ProposalReason = "When looking for full tokens, has works better, since it doesn't look for substrings"
                //has does a search by the index of the columns
                //and does a search on whole words 
                //and therefore it is more efficient than contains
            }, 
            new ProposalScheme
            {
                //עושה חיפוש לפי האינדקס של העמודות has
                //עושה חיפוש על מילים שלמות has
                //ולכן זה יעיל יותר
                sourceOperator = "Contains_cs",
                ProposalOptimalOperator = "has_cs",
                ProposalReason = "Use contains_cs instead of has_cs because has_cs does a search by the index of the columns and does a search on whole words and therefore it is more efficient than contains"
            }, 
            new ProposalScheme
            {
                // רגיש לאותיות גדולות לכן החיפוש יהיה ממוקד יותר ומהיר יותר cs ערך בסיומת
                sourceOperator = "Has",
                ProposalOptimalOperator = "has_cs",
                ProposalReason = "Use has instead of has_cs because the value with the suffix cs is case sensitive so the search will be more focused and faster than has"
            }, 
            new ProposalScheme
            {   //רגיש לאותיות גדולות in~
                //ולכן זה יעיל יותר
                sourceOperator = "In~",
                ProposalOptimalOperator = "in",
                ProposalReason = "Use in~ instead of in because in~ is case sensitive, so the search is more focused and faster"
            }, 
            new ProposalScheme//4
            {  // יושמטו null שורות בטבלה השמאלית שאין להם התאמה בטבלה הימנית במקום להיות  
                sourceOperator = "Lookup",
                ProposalOptimalOperator = "lookup kind=leftouter",
                ProposalReason = "Use lookup kind=leftouter instead of lookup because kind=leftouter put Null rows in the left table that do not have a match in the right table will be omitted instead of being"
            }, 
            new ProposalScheme
            {  
                sourceOperator = "Join",
                ProposalOptimalOperator = "lookup",
                ProposalReason = "Use lookup instead of join because join needs more memory so lookup will be faster"
            }, 
            new ProposalScheme
            {    
                sourceOperator = "Join",
                ProposalOptimalOperator = "left join",
                ProposalReason = "if the left table is always smaller than the other, use it as the left (piped) side of the join"
            }, 
            new ProposalScheme
            {  
                sourceOperator = "Join",
                ProposalOptimalOperator = "right join",
                ProposalReason = "if the right table is always smaller than the other, use it as the left (piped) side of the join"
            }, 
       
            new ProposalScheme
            {    
                sourceOperator = "Search",
                ProposalOptimalOperator = "where",
                ProposalReason = "Prefer to filter inside the search operator"
            },
               new ProposalScheme
            {   //???
                sourceOperator = "Has_any",
                ProposalOptimalOperator = "has",
                ProposalReason = ""
            },
        };}
    }
}