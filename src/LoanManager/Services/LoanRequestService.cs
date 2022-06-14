using CustomerManager.Model;
using LoanManager.Interface;
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
        private readonly IDecisionService _decisionService;

        public LoanRequestService(ILoanRequestRepository loanRequestRepository, IDecisionRepository decisionRepository, IRejectRepository rejectRepository, IDecisionService decisionService)
        {
            _loanRequestRepository = loanRequestRepository;
            _decisionRepository = decisionRepository;
            _rejectRepository = rejectRepository;
            _decisionService = decisionService;
        }

        public async Task<List<LoanApplicationItem>> GetByUserIdAsync(string userId, CancellationToken cancellationToken)
        {
            return await _loanRequestRepository.GetByUserIdAsync(userId, cancellationToken);                                    }

        public async Task<LoanApplicationItem> GetByLoanIdAsync(string loanId, CancellationToken cancellationToken)
        {
            return await _loanRequestRepository.GetByLoanIdAsync(loanId, cancellationToken);
        }

        public async Task<LoanApplicationRespons> CreateAsync(LoanApplicationCreateRequest request, CancellationToken cancellationToken)
        {
            //Här borde man kolla en token om användaren är behörig att lägg upp ett lån. 

            var decision = await _decisionService.ApplayLoanAsync(request, cancellationToken);

            LoanRequestRespons loanRequestRespons;
                
            if (decision.Approved)
            {
                loanRequestRespons = await _loanRequestRepository.CreateAsync(request, cancellationToken);
            }
            else
            {
                loanRequestRespons = await _rejectRepository.CreateAsync(request, cancellationToken);
            }

            await SaveLoanDecision(loanRequestRespons.Id, decision.Approved, decision.Decisions, cancellationToken);

            if (!decision.Approved)
            {
                return new LoanApplicationRespons(loanRequestRespons.Id, decision.Approved, decision.Decisions);
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

            var decision = await _decisionService.ApplayLoanAsync(item, cancellationToken);

            LoanRequestRespons loanRequestRespons = null;

            if (decision.Approved)
            {
                loanRequestRespons = await _loanRequestRepository.UpdateAsync(item, cancellationToken);
            }
            else
            {
                loanRequestRespons = await _rejectRepository.CreateAsync(item, cancellationToken);
            }


            await SaveLoanDecision(item.Id, decision.Approved, decision.Decisions, cancellationToken);

            if (!decision.Approved)
            {
                return new LoanApplicationRespons(loanRequestRespons.Id, decision.Approved, decision.Decisions);
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

        
        private async Task SaveLoanDecision(string id, bool isApproved, List<string> decision, CancellationToken cancellationToken)
        {
            await _decisionRepository.CreateAsync(new DecisionItem( id, DateTime.UtcNow, isApproved, decision), cancellationToken);
        }
    }
}
