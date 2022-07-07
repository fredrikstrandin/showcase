using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanManager.Model
{
    public class LoanApplicationItem
    {   
        public LoanApplicationItem(string id, LoanType type, int? amount, int? duration)
        {
            Id = id;
            Type = type;
            Amount = amount;
            Duration = duration;
        }
        public string Id { get; set; }
        
        public LoanType Type { get; }
        public int? Amount { get; }
        //Antar att man inte går in på delar av månader
        public int? Duration { get; }        
    }
}
