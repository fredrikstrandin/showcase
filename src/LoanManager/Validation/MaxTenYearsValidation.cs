using LoanManager.Model;
using LoanManager.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LoanManager.Validation
{
    public class MaxTenYearsValidation : ILoanApplicationValidation
    {
        public bool IsLoanToValidation(LoanType type)
        {
            return type == LoanType.PrivateLoan;
        }

        public Task<LoanValidationRespons> Validation(LoanApplicationItem request, CancellationToken cancellationToken)
        {
            if (request.Duration.HasValue && request.Duration.Value > 120)
            {
                return Task.FromResult(new LoanValidationRespons(false, "Loan duration is over 10 years"));
            }
            else
            {
                return Task.FromResult(new LoanValidationRespons(true));
            }
        }
    }
}
