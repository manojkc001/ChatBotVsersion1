using System;
using System.Collections.Generic;
using Bot_Application.Configuration;

namespace Bot_Application
{
    public class CreditCardTypes
    {
        public Classic ClassicCard { get; set; }
        public Platinum PlatinumCard { get; set; }
    }

    public class Platinum 
    {
        public InfoForCards PlatinumInformation;
    }

    public class Classic
    {
        public InfoForCards ClassicInformation;
    }
    public class InfoForCards
    {
        public string AnnualFees { get; set; }
        public string Eligibility { get; set; }
        public string InterestRatesForCards { get; set; }
        public string CardLimit { get; set; }
    }

}