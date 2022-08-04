using Kusto.Language.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueryProfiler
{
   public class ProfileScheme
    {
        
        public int SearchCounter { get; set; }
        public int ExtendCounter { get; set; }
        public int JoinCounter { get; set; }
        public int LookupCounter { get; set; }
        public int ProjectCounter { get; set; }
        public int InCounter { get; set; }
        public int ProjectAwayCounter { get; set; }
        public int ProjectKeepCounter { get; set; }
        public int ContainsCounter { get; set; }
        public int ContainsCsCounter { get; set; }
        public int Has_csCounter { get; internal set; }
        public int Contains_csCounter { get; internal set; }
        public int HasCounter { get; internal set; }
        public int WhereCounter { get; internal set; }
    }
}
