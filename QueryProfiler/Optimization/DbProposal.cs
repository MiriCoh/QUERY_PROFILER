using System;
using System.Collections.Generic;
using System.Text;

namespace QueryProfiler
{
    class DbProposal
    {
        public static void DB()
        {
            var db = new List<ProposalScheme>();
            var proposal1 = new ProposalScheme
            {  
                // רגיש לאותיות גדולות לכן החיפוש יהיה ממוקד יותר ומהיר יותר cs ערך בסיומת
                sourceOperator = "contains",
                ProposalOptimalOperator = "contains_cs",
                WyhItsBetter = "Use contains_cs instead of contains because the value with a cs suffix is ​​case sensitive so the search will be more focused and faster than contains"
            };
            var proposal2 = new ProposalScheme
            {  
                //עושה חיפוש לפי האינדקס של העמודות has
                //עושה חיפוש על מילים שלמות has
                //ולכן זה יעיל יותר
                sourceOperator = "contains",
                ProposalOptimalOperator = "has",
                WyhItsBetter = "When looking for full tokens, has works better, since it doesn't look for substrings"
                //has does a search by the index of the columns
                //and does a search on whole words 
                //and therefore it is more efficient than contains

            };
            var proposal3 = new ProposalScheme
            {
                //עושה חיפוש לפי האינדקס של העמודות has
                //עושה חיפוש על מילים שלמות has
                //ולכן זה יעיל יותר
                sourceOperator = "contains_cs",
                ProposalOptimalOperator = "has_cs",
                WyhItsBetter = "Use contains_cs instead of has_cs because has_cs does a search by the index of the columns and does a search on whole words and therefore it is more efficient than contains"
            };
            var proposal4 = new ProposalScheme
            {
                // רגיש לאותיות גדולות לכן החיפוש יהיה ממוקד יותר ומהיר יותר cs ערך בסיומת
                sourceOperator = "has",
                ProposalOptimalOperator = "has_cs",
                WyhItsBetter = "Use has instead of has_cs because the value with the suffix cs is case sensitive so the search will be more focused and faster than has"
            };
            var proposal5 = new ProposalScheme
            {
                sourceOperator = "has_any",
                ProposalOptimalOperator = "has",
                WyhItsBetter = "if one table is always smaller than the other, use it as the left (piped) side of the join"
            };

            var proposal6 = new ProposalScheme
            {   //רגיש לאותיות גדולות in~
                //ולכן זה יעיל יותר
                sourceOperator = "in",
                ProposalOptimalOperator = "in~",
                WyhItsBetter = "Use in~ instead of in because in~ is case sensitive, so the search is more focused and faster"
            };
            var proposal7 = new ProposalScheme
            {  // יושמטו null שורות בטבלה השמאלית שאין להם התאמה בטבלה הימנית במקום להיות  
                sourceOperator = "lookup",
                ProposalOptimalOperator = "lookup kind=leftouter",
                WyhItsBetter = "Use lookup kind=leftouter instead of lookup because kind=leftouter put Null rows in the left table that do not have a match in the right table will be omitted instead of being"
            };
        var proposal8 = new ProposalScheme
            {    //
                sourceOperator = "join",
                ProposalOptimalOperator = "left join",
                WyhItsBetter = "if one table is always smaller than the other, use it as the left (piped) side of the join"
            };
            var proposal9 = new ProposalScheme
            {    //
                sourceOperator = "join",
                ProposalOptimalOperator = "right join",
                WyhItsBetter = "use right table if one table is always smaller than the other, use it as the left (piped) side of the join"
            };
            var proposal10 = new ProposalScheme
            {    //
                sourceOperator = "search",
                ProposalOptimalOperator = "where",
                WyhItsBetter = "Prefer to filter inside the search operator"
            };
        }
    }
}
