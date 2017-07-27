using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot_Application.Configuration
{
   public interface ICards
    {
        string EligibilityCriteria();
        string InterestRates();
        int AnnualFee();
    }
}
