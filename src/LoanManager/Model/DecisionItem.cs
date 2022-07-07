using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanManager.Model
{
    public class DecisionItem
    {
        public DecisionItem(string userId, string loanId, DateTime created, bool aproved, List<string> decisions)
        {
            UserId = userId;
            LoanId = loanId;
            Created = created;
            Approved = aproved;
            Decisions = decisions;
        }

        public string UserId { get; }
        public string LoanId { get; }
        public DateTime Created { get; }
        public bool Approved { get; }
        public List<string> Decisions { get; }
    }
}
