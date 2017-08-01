using System;
using System.Collections.Generic;

namespace Bot_Application
{
    public class CreditCardTypes
    {
        public Classic ClassicCard { get; set; }
        public Platinum PlatinumCard { get; set; }
    }

    public class Platinum 
    {
        public CardInformation PlatinumInformation;
    }

    public class Classic
    {
        public CardInformation ClassicInformation;
    }
    public class CardInformation
    {
        public string AnnualFees { get; set; }
        public string Eligibility { get; set; }
        public string InterestRatesForCards { get; set; }
        public string CardLimit { get; set; }
        public string Rewards { get; set; }
        public string Fee { get; set; }
    }
    public class TagEntity
    {
        public bool CreditCard { get; set; }
        public bool Classic { get; set; }
        public bool Platinum { get; set; }
        public bool Eligibility { get; set; }
        public bool AnnualFee { get; set; }
        public bool InterestRate { get; set; }
        public bool CardLimit { get; set; }
        public bool CreditLimit { get; set; }
        public bool CCInformation { get; set; }
        public bool Rewards { get; set; }
        public bool Fee { get; set; }
        public bool Features { get; set; }
        public bool AnnualFees { get; set; }
        public bool Benefits { get; set; }
        public bool InfoOnCard { get; set; }
    }

}