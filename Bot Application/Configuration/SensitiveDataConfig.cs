using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Bot_Application
{
    [Serializable()]
    public class SensitiveDataConfig
    {
        [XmlElement("Node1")]
        public string Node1 { get; set; }

        [XmlElement("Node2")]
        public string Node2 { get; set; }

        [XmlElement("Node3")]
        public string Node3 { get; set; }

        [XmlElement("Node4")]
        public string Node4 { get; set; }

        [XmlElement("Node5")]
        public string Node5 { get; set; }

    }
}