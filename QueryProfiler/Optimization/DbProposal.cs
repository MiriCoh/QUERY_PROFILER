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
                WyhItsBetter = ""
            };
            var proposal2 = new ProposalScheme
            {  
                //עושה חיפוש לפי האינדקס של העמודות has
                //עושה חיפוש על מילים שלמות has
                //ולכן זה יעיל יותר
                sourceOperator = "contains",
                ProposalOptimalOperator = "has",
                WyhItsBetter = "When looking for full tokens, has works better, since it doesn't look for substrings"

            };
            var proposal3 = new ProposalScheme
            {
                //עושה חיפוש לפי האינדקס של העמודות has
                //עושה חיפוש על מילים שלמות has
                //שתיהם רגישות לאותיות גדולות
                //ולכן זה יעיל יותר
                sourceOperator = "contains_cs",
                ProposalOptimalOperator = "has_cs",
                WyhItsBetter = "if one table is always smaller than the other, use it as the left (piped) side of the join"
            };
            var proposal4 = new ProposalScheme
            {
                // רגיש לאותיות גדולות לכן החיפוש יהיה ממוקד יותר ומהיר יותר cs ערך בסיומת
                sourceOperator = "has",
                ProposalOptimalOperator = "has_cs",
                WyhItsBetter = "if one table is always smaller than the other, use it as the left (piped) side of the join"
            };
            var proposal5 = new ProposalScheme
            {
                sourceOperator = "in",
                ProposalOptimalOperator = "has_cs",
                WyhItsBetter = "if one table is always smaller than the other, use it as the left (piped) side of the join"
            };

            var proposal6 = new ProposalScheme
            {   //רגיש לאותיות גדולות in~
                //ולכן זה יעיל יותר
                sourceOperator = "in",
                ProposalOptimalOperator = "in~",
                WyhItsBetter = "Use case-sensitive operators when possible"
            };
            var proposal7 = new ProposalScheme
            {  // יושמטו null שורות בטבלה השמאלית שאין להם התאמה בטבלה הימנית במקום להיות  
                sourceOperator = "lookup",
                ProposalOptimalOperator = "lookup kind=leftouter",
                WyhItsBetter = "Use in instead of left semi join for filtering by a single colum"
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
                WyhItsBetter = "if one table is always smaller than the other, use it as the left (piped) side of the join"
            };

        }
    }
}
