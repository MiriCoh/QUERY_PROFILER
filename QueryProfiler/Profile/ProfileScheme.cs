using System.Collections.Generic;
namespace QueryProfiler
{
   public class ProfileScheme
    {  
        public int JoinCounter { get; set; }
        public int UnionCounter { get; set; }
        public int LookupCounter { get; set; }
        public int MvExpandCounter { get; set; }
        public int InCounter { get; set; }
        public List<string> Tables { get; set; }
    }
}
