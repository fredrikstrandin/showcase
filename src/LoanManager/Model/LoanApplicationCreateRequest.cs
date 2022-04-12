using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanManager.Model
{
    public class LoanApplicationCreateRequest
    {
        public string UserId { get; set; }
        public LoanType Type { get; set; }
        public int Amount { get; set; }
        //Antar att man inte går in på delar av månader
        public int Duration { get; set; }

        public static implicit operator LoanApplicationItem(LoanApplicationCreateRequest entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new LoanApplicationItem()
            {
                UserId = entity.UserId.ToString(),
                Amount = entity.Amount,
                Duration = entity.Duration,
                Type = entity.Type,                
            };
        }
    }
}
