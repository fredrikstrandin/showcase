using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanManager.Model
{
    public class LoanValidationRespons
    {
        public LoanValidationRespons(bool isApprovd, string decision = null)
        {
            Decision = decision;
            IsApprovd = isApprovd;
        }

        public bool IsApprovd { get; }
        public string Decision { get; }

    }
}
