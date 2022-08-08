using Kusto.Language.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueryProfiler
{
   public class ProfileScheme
    {     // Operators
        public int ContainsCounter { get; set; }
        public int Contains_csCounter { get; internal set; }
        public int Has_csCounter { get; internal set; }
        public int HasCounter { get; internal set; }
        public int WhereCounter { get; internal set; }
    }
}
