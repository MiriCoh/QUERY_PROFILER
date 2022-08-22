using System.Collections.Generic;
using System.Xml.Serialization;
namespace QueryProfiler
{
    [XmlRoot(elementName: "ProposalsOptimizations")]
    public class ProposalsOptimizations
    {
        [XmlElement(elementName: "ProposalScheme")]
        public List<ProposalScheme> ProposalsOptimization = new List<ProposalScheme>();

    }
    [XmlRoot(elementName: "ProposalScheme")]
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
    public class ProposalSchemaEqualityComparer : IEqualityComparer<ProposalScheme>
    {
        public bool Equals(ProposalScheme ps1, ProposalScheme ps2)
        {
            if (ReferenceEquals(ps1, ps2))
                return true;
            if (ReferenceEquals(ps1, null) && ReferenceEquals(ps2, null))
                return false;

            return (ps1.OperatorPosition == ps2.OperatorPosition
                 && ps1.ProposalOptimalOperator == ps2.ProposalOptimalOperator
                 && ps1.ProposalReason == ps2.ProposalReason
                 && ps1.SourceOperator == ps2.SourceOperator);
        }
        public int GetHashCode(ProposalScheme obj)
        {
            if (ReferenceEquals(obj, null))
                return 0;
            int hashCodeName = obj.SourceOperator == null ? 0 : obj.SourceOperator.GetHashCode();
            int hasCodeAge = obj.ProposalReason.GetHashCode();
            return hashCodeName ^ hasCodeAge;
        }
    }
}
