using System.Collections.Generic;
using System.Xml.Serialization;

namespace QueryProfiler
{
    [XmlRoot(elementName: "ProposalsOptimizations")]
    public class ProposalsOptimizations
    {
        [XmlElement("ProposalScheme")]
        public List<ProposalScheme> ProposalsOptimization = new List<ProposalScheme>();

    }
    [XmlRoot("ProposalScheme")]
    public class ProposalScheme
    {
        [XmlElement]
        public string sourceOperator { get; set; }
        [XmlElement]
        public string ProposalOptimalOperator { get; set; }
        [XmlElement]
        public string ProposalReason { get; set; }
    }
}
