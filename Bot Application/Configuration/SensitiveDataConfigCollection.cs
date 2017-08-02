using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Bot_Application
{
    [Serializable]
    [XmlRoot("ProductConfig")]
    public class SensitiveDataConfigCollection
    {
        [XmlArray("Products")]
        [XmlArrayItem("Product", typeof(SensitiveDataConfig))]
        public List<SensitiveDataConfig> Items { get; set; }
    }
}