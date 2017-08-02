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
        [XmlElement("CreditCard")]
        public CreditCard CreditCard { get; set; }
    }
}