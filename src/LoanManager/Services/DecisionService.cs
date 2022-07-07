using LoanManager.Interface;
using LoanManager.Model;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LoanManager.Services
{
    public class DecisionService : IDecisionService
    {
        private readonly IEnumerable<ILoanApplicationValidation> _loanRequestValidation;
        private readonly IGenerateId _generateId;

        public DecisionService(IEnumerable<ILoanApplicationValidation> loanRequestValidation, IGenerateId generateId)
        {
            _loanRequestValidation = loanRequestValidation;
            _generateId = generateId;
        }

        public async Task<DecisionItem> ApplayLoanAsync(LoanApplicationCreateItem item, CancellationToken cancellationToken)
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

            return new DecisionItem(item.UserId, _generateId.GenerateNewId(), DateTime.UtcNow, isApproved, reasons);
        }
    }
}
