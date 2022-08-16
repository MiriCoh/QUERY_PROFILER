using System.Collections.Generic;
using System.Xml.Serialization;

namespace QueryProfiler
{
    [XmlRoot(elementName: "ProposalsOptimizations")]
    public class ProposalsOptimizations
    {
        [XmlElement(elementName:"ProposalScheme")]
        public List<ProposalScheme> ProposalsOptimization = new List<ProposalScheme>();

    }
    [XmlRoot(elementName:"ProposalScheme")]
    public class ProposalScheme
    {
        [XmlElement(elementName: "SourceOperator")]
        public string SourceOperator { get; set; }
        [XmlElement(elementName: "ProposalOptimalOperator")]
        public string ProposalOptimalOperator { get; set; }
        [XmlElement(elementName: "ProposalReason")]
        public string ProposalReason { get; set; }
        public int OperatorPosition { get; set; }
    }
}
