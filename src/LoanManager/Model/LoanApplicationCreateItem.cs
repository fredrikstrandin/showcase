using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanManager.Model
{
    public class LoanApplicationCreateItem
    {
        public LoanApplicationCreateItem(string userId, LoanType type, int? amount, int? duration)
        {
            UserId = userId;
            Type = type;
            Amount = amount;
            Duration = duration;
        }
        public string UserId { get; set; }
        public LoanType Type { get; set; }
        public int? Amount { get; set; }
        //Antar att man inte går in på delar av månader
        public int? Duration { get; set; }        
    }
}
