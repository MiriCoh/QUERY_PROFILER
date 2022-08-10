using System.Collections.Generic;
namespace QueryProfiler
{
    class DbProposal
    {
       public static List<ProposalScheme> dbProposalOptimization = new List<ProposalScheme>();
        public static void InitializingDatabaseOptimalOperators()
        {
            dbProposalOptimization=new List<ProposalScheme>
            {
            new ProposalScheme
            {    // רגיש לאותיות גדולות לכן החיפוש יהיה ממוקד יותר ומהיר יותר cs ערך בסיומת
                sourceOperator = "contains",
                ProposalOptimalOperator = "contains_cs",
                ProposalReason  = "Usecontains_cs instead of contains because the value with a cs suffix is ​​case sensitive so the search will be more focused and faster than contains"
            },//0
            new ProposalScheme
            {
                 //עושה חיפוש לפי האינדקס של העמודות has
                //עושה חיפוש על מילים שלמות has
                //ולכן זה יעיל יותר
                sourceOperator = "contains",
                ProposalOptimalOperator = "has",
                ProposalReason = "When looking for full tokens, has works better, since it doesn't look for substrings"
                //has does a search by the index of the columns
                //and does a search on whole words 
                //and therefore it is more efficient than contains
            }, //1
            new ProposalScheme
            {
                //עושה חיפוש לפי האינדקס של העמודות has
                //עושה חיפוש על מילים שלמות has
                //ולכן זה יעיל יותר
                sourceOperator = "contains_cs",
                ProposalOptimalOperator = "has_cs",
                ProposalReason = "Use contains_cs instead of has_cs because has_cs does a search by the index of the columns and does a search on whole words and therefore it is more efficient than contains"
            }, //2
            new ProposalScheme
            {
                // רגיש לאותיות גדולות לכן החיפוש יהיה ממוקד יותר ומהיר יותר cs ערך בסיומת
                sourceOperator = "has",
                ProposalOptimalOperator = "has_cs",
                ProposalReason = "Use has instead of has_cs because the value with the suffix cs is case sensitive so the search will be more focused and faster than has"
            }, //3
            new ProposalScheme
            {   //???
                sourceOperator = "has_any",
                ProposalOptimalOperator = "has",
                ProposalReason = "if one table is always smaller than the other, use it as the left (piped) side of the join"
            }, //4
            new ProposalScheme
            {   //רגיש לאותיות גדולות in~
                //ולכן זה יעיל יותר
                sourceOperator = "in~",
                ProposalOptimalOperator = "in",
                ProposalReason = "Use in~ instead of in because in~ is case sensitive, so the search is more focused and faster"
            }, //5
            new ProposalScheme//4
            {  // יושמטו null שורות בטבלה השמאלית שאין להם התאמה בטבלה הימנית במקום להיות  
                sourceOperator = "lookup",
                ProposalOptimalOperator = "lookup kind=leftouter",
                ProposalReason = "Use lookup kind=leftouter instead of lookup because kind=leftouter put Null rows in the left table that do not have a match in the right table will be omitted instead of being"
            }, //6
            new ProposalScheme
            {    
                sourceOperator = "join",
                ProposalOptimalOperator = "left join",
                ProposalReason = "if one table is always smaller than the other, use it as the left (piped) side of the join"
            }, //7
            new ProposalScheme
            {    //
                sourceOperator = "join",
                ProposalOptimalOperator = "right join",
                ProposalReason = "Use right table if one table is always smaller than the other, use it as the left (piped) side of the join"
            }, //8
                 new ProposalScheme
            {    //
                sourceOperator = "join",
                ProposalOptimalOperator = "lookup",
                ProposalReason = "Use lookup instead of join because join needs more memory so lookup will be faster"
            }, //9
            new ProposalScheme
            {    //
                sourceOperator = "search",
                ProposalOptimalOperator = "where",
                ProposalReason = "Prefer to filter inside the search operator"
            }  //10
        };}
    }
}