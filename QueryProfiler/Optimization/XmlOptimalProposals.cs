using System.Xml.Serialization;
using System.IO;
using QueryProfiler.Properties;
using System.Xml;
using System.Xml.Linq;
using System;

namespace QueryProfiler.Optimization
{
    public static class XmlOptimalProposals
    {
        //public static string CONFIG_FNAME = "../../../Optimization/XmlProposal.xml";
        public static string CONFIG_FNAME = "XmlProposal.xml";

        public static ProposalsOptimizations GetProposalsOptimization()
        {
            //Object ret = Resources.ResourceManager.GetObject("XmlProposal");
            //var myIcon = Resources.XmlProposal;
            ////XmlTextReader reader = null;
            //XmlTextReader reader = null;// new XmlTextReader(Resources.Culture.Name);
            //reader = new XmlTextReader(new StringReader((string)ret));
            //FileStream SubSetupStream = new FileStream(myIcon,FileMode.Open);
            //if (SubSetupStream.Position > 0)
            //{
            //    SubSetupStream.Position = 0;
            //}
            XmlDocument quoteDocument = new XmlDocument();
            quoteDocument.LoadXml(Resources.XmlProposal);
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
    }
}
