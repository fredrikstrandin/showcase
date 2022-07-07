using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanManager.Model
{
    public class LoanApplicationCreateRequest
    {
        public LoanType Type { get; set; }
        public int Amount { get; set; }
        //Antar att man inte går in på delar av månader
        public int Duration { get; set; }
    }
}
