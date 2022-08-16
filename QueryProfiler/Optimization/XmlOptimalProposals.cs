using System.Xml.Serialization;
using System.IO;
using System;

namespace QueryProfiler.Optimization
{
    public static class XmlOptimalProposals
    {

        public static string CONFIG_FNAME = "XmlProposal.xml";

        public static ProposalsOptimizations GetProposalsOptimization()
        {
            if (!File.Exists(CONFIG_FNAME)) 
            {
                using (var fs = new FileStream(CONFIG_FNAME, FileMode.Create))
                {
                    var xs = new XmlSerializer(typeof(ProposalsOptimizations));
                    var sxml = new ProposalsOptimizations();
                    xs.Serialize(fs, sxml);
                    return sxml;
                }
            }
            else 
            {
                var xRoot = new XmlRootAttribute("ProposalsOptimizations");
                using (var fs = new FileStream(CONFIG_FNAME, FileMode.Open))
                {
                    var xs = new XmlSerializer(typeof(ProposalsOptimizations),xRoot);
                    var sc = (ProposalsOptimizations)xs.Deserialize(fs);
                    return sc;
                }
            }
        }

        public static bool SaveProposalScheme(ProposalScheme config)
        {
            if (!File.Exists(CONFIG_FNAME)) return false; // don't do anything if file doesn't exist

            using (FileStream fs = new FileStream(CONFIG_FNAME, FileMode.Open))
            {
                var xs = new XmlSerializer(typeof(ProposalScheme));
                xs.Serialize(fs, config);
                return true;
            }
        }
    

  

    }
}
