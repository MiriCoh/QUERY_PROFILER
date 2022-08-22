using System.Xml.Serialization;
using System.Xml.Linq;

namespace QueryProfiler.Optimization
{
    public static class XmlOptimalProposals
    {
        public static ProposalsOptimizations GetProposalsOptimization()
        {
            XDocument doc = XDocument.Parse(Resources.Resources.XMLProposals);
            using (var reader = doc.CreateReader())
            {
                var xs = new XmlSerializer(typeof(ProposalsOptimizations));
                var xd = xs.Deserialize(reader) as ProposalsOptimizations;
                return xd;
            }
        }
    }
}
