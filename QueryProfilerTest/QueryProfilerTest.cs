using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nancy.Json;
using QueryProfiler;
using System.Collections.Generic;
namespace QueryProfilerTest
{
    [TestClass]
    public class QueryProfilerTest
    {
        [TestMethod]
        public void TestGetProfileToQuery()
        {
            var query = "Table1 | join (Table2) on CommonColumn, $left.Col1 == $right.Col2";
            var actual = QueryProfiler.Profile.ProfileAnalyzer.GetProfile(query); 
            var expected = new ProfileScheme
            {
                JoinCounter = 1,
                UnionCounter = 0,
                LookupCounter = 0,
                MvExpandCounter = 0,
                InCounter = 0,
                Tables = new List<string>
                {
                    "Table1",
                    "Table2"
                }
            };
            Assert.AreEqual(expected,actual);
        }
    }

}