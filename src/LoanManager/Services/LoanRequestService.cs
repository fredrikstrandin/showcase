using CustomerManager.Model;
using LoanManager.Model;
using LoanManager.Repository;
using LoanManager.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LoanManager.Services
{
    public class LoanRequestService : ILoanRequestService
    {
        private readonly ILoanRequestRepository _loanRequestRepository;
        private readonly IDecisionRepository _decisionRepository;
        private readonly IRejectRepository _rejectRepository; 
        private readonly IEnumerable<ILoanApplicationValidation> _loanRequestValidation;

        public LoanRequestService(ILoanRequestRepository loanRequestRepository, IEnumerable<ILoanApplicationValidation> loanRequestValidation, 
            IDecisionRepository decisionRepository, IRejectRepository rejectRepository)
        {
            _loanRequestRepository = loanRequestRepository;
            _loanRequestValidation = loanRequestValidation;
            _decisionRepository = decisionRepository;
            _rejectRepository = rejectRepository;
        }

        public async Task<List<LoanApplicationItem>> GetByUserIdAsync(string userId, CancellationToken cancellationToken)
        {
            return await _loanRequestRepository.GetByUserIdAsync(userId, cancellationToken);
        }

        public async Task<LoanApplicationItem> GetByLoanIdAsync(string loanId, CancellationToken cancellationToken)
        {
            return await _loanRequestRepository.GetByLoanIdAsync(loanId, cancellationToken);
        }

        public async Task<LoanApplicationRespons> CreateAsync(LoanApplicationCreateRequest request, CancellationToken cancellationToken)
        {
            //Här borde man kolla en token om användaren är behörig att lägg upp ett lån. 

            var decision = await LoanDecision(request, cancellationToken);

            LoanRequestRespons loanRequestRespons;
                
            if (decision.IsApproved)
            {
                loanRequestRespons = await _loanRequestRepository.CreateAsync(request, cancellationToken);
            }
            else
            {
                loanRequestRespons = await _rejectRepository.CreateAsync(request, cancellationToken);
            }

            await SaveLoanDecision(loanRequestRespons.Id, decision.IsApproved, decision.Reasons, cancellationToken);

            if (!decision.IsApproved)
            {
                return new LoanApplicationRespons(loanRequestRespons.Id, decision.IsApproved, decision.Reasons);
            }

            if (!decision.IsApproved)
            {
                return new LoanApplicationRespons(null, decision.IsApproved, decision.Reasons);
            }

            if (loanRequestRespons.IsSuccess)
            {
                return new LoanApplicationRespons(loanRequestRespons.Id, loanRequestRespons.IsSuccess);
            }
            else
            {
                return new LoanApplicationRespons(null, false, new List<string>() { "Problems saving your loan application." });
            }
        }

        public async Task<LoanApplicationRespons> UpdateAsync(LoanApplicationUpdateRequest request, CancellationToken cancellationToken)
        {
            //Här borde man kolla en token om användaren är behörig att ändra lånet. 

            LoanApplicationItem item = await _loanRequestRepository.GetByLoanIdAsync(request.Id, cancellationToken);

            if(item == null)
            {
                return new LoanApplicationRespons(null, false, new List<string> { "The user does not exist." });
            }

            //Uppdaterar med nya värdena
            if (request.Amount.HasValue)
            {
                item.Amount = request.Amount;
            }

            if (request.Duration.HasValue)
            {
                item.Duration = request.Duration;
            }

            var decision = await LoanDecision(item, cancellationToken);

            LoanRequestRespons loanRequestRespons = null;

            if (decision.IsApproved)
            {
                loanRequestRespons = await _loanRequestRepository.UpdateAsync(item, cancellationToken);
            }
            else
            {
                loanRequestRespons = await _rejectRepository.CreateAsync(item, cancellationToken);
            }


            await SaveLoanDecision(item.Id, decision.IsApproved, decision.Reasons, cancellationToken);

            if (!decision.IsApproved)
            {
                return new LoanApplicationRespons(loanRequestRespons.Id, decision.IsApproved, decision.Reasons);
            }

            if (loanRequestRespons.IsSuccess)
            {
                return new LoanApplicationRespons(loanRequestRespons.Id, loanRequestRespons.IsSuccess);
            }
            else
            {
                return new LoanApplicationRespons(null, false, new List<string>() { "Problems saving your loan application." });
            }
        }

        public async Task<LoanApplicationRespons> DeleteAsync(string loanId, CancellationToken cancellationToken)
        {
            var ret = await _loanRequestRepository.DeleteAsync(loanId, cancellationToken);

            if (ret.IsSuccess)
            {
                return new LoanApplicationRespons(ret.Id, ret.IsSuccess);
            }
            else
            {
                return new LoanApplicationRespons(ret.Id, ret.IsSuccess, new List<string>() { "Problems deleting your loan application." });
            }
        }

        //Detta borde vara en tjänst om inte annat för att lätt kunna testa men detta är bara demo
        private async Task<(bool IsApproved, List<string> Reasons)> LoanDecision(LoanApplicationItem item, CancellationToken cancellationToken)
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

            return (isApproved, reasons);
        }

        private async Task SaveLoanDecision(string id, bool isApproved, List<string> decision, CancellationToken cancellationToken)
        {
            await _decisionRepository.CreateAsync(new DecisionItem() { LoanId = id, Created = DateTime.UtcNow, Aproved = isApproved, Decisions = decision }, cancellationToken);
        }
    }
}
