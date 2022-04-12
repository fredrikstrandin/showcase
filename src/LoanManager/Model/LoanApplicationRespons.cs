using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanManager.Model
{
    public class LoanApplicationRespons
    {
        public LoanApplicationRespons(string loanId, bool isSuccess, List<string> decision = null)
        {
            LoanId = loanId;
            IsSuccess = isSuccess;
            Decision = decision;
        }

        public string LoanId { get; }
        public bool IsSuccess { get; }
        public List<string> Decision { get; }
    }
}
