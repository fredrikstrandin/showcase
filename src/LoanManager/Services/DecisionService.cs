using LoanManager.Interface;
using LoanManager.Model;
using LoanManager.Validation;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LoanManager.Services
{
    public class DecisionService : IDecisionService
    {
        private readonly IEnumerable<ILoanApplicationValidation> _loanRequestValidation;

        public DecisionService(IEnumerable<ILoanApplicationValidation> loanRequestValidation)
        {
            _loanRequestValidation = loanRequestValidation;
        }

        public async Task<DecisionItem> ApplayLoanAsync(LoanApplicationItem item, CancellationToken cancellationToken)
        {
            bool isApproved = true;
            List<string> reasons = new List<string>();

            foreach (ILoanApplicationValidation validation in _loanRequestValidation)
            {
                if (validation.IsLoanToValidation(item.Type))
                {
                    var validationRespons = await validation.Validation(item, cancellationToken);

                    if (!validationRespons.IsApprovd)
                    {
                        isApproved = validationRespons.IsApprovd;
                        reasons.Add(validationRespons.Decision);
                    }
                }
            }

            return new DecisionItem(item.Id, DateTime.UtcNow, isApproved, reasons);
        }
    }
}
