using Kusto.Language.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueryProfiler
{
   public class ProfileScheme
    {   // Operators
        public int JoinCounter { get; set; }
        public int LookupCounter { get; set; }
        public int InCounter { get; set; }
    }
}
