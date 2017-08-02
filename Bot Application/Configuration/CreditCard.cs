using System;
using System.Collections.Generic;
using Bot_Application.Configuration;
using System.Xml.Serialization;

namespace Bot_Application
{
    [Serializable()]
    public class CreditCard

    {
        [XmlElement("CustomerCare")]
        public string CustomerCare { get; set; }
        [XmlElement("CardType")]
        public string CardType { get; set; }
        [XmlElement("Features")]
        public string Features { get; set; }
        [XmlElement("Benefits")]
        public string Benefits { get; set; }
        [XmlElement("Cancel")]
        public string Cancel { get; set; }
        [XmlElement("Apply")]
        public string Apply { get; set; }
        [XmlElement("ChangePin")]
        public string ChangePin { get; set; }
        [XmlElement("OrangeOne")]
        public string OrangeOne { get; set; }
        [XmlElement("InterestRates")]
        public string InterestRates { get; set; }
        [XmlElement("AnnualFee")]
        public Classic AnnualFee { get; set; }
        [XmlElement("CardLimit")]
        public string CardLimit { get; set; }
        [XmlElement("Rewards")]
        public string Rewards { get; set; }
        [XmlElement("Classic")]
        public Classic Classic { get; set; }
        [XmlElement("Platinum")]
        public Platinum Platinum { get; set; }  
    }
}