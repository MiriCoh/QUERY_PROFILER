using Kusto.Language.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueryProfiler.Profile
{
   public class OperatorSchema
    {
        public SyntaxKind Operator { get; set; }
        public string Kind { get; set; }
    }
}
