using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Bot_Application
{
    [Serializable]
    [XmlRoot("UserConfig")]
    public class SensitiveDataConfigCollection
    {
        [XmlArray("Users")]
        [XmlArrayItem("User",typeof(SensitiveDataConfig))]
        public List<SensitiveDataConfig> Items { get; set; }
    }
}