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

        public async Task<LoanApplicationItem> GetByLoanIdAsync(string userId, string loanId, CancellationToken cancellationToken)
        {
            return await _loanRequestRepository.GetByLoanIdAsync(userId, loanId, cancellationToken);
        }

        public async Task<LoanApplicationRespons> CreateAsync(string userId, LoanApplicationCreateRequest request, CancellationToken cancellationToken)
        {
            //Här borde man kolla en token om användaren är behörig att lägg upp ett lån. 

            var decision = await _decisionService.ApplayLoanAsync(new LoanApplicationCreateItem(userId, request.Type, request.Amount, request.Duration), cancellationToken);

            LoanRequestRespons loanRequestRespons;
                
            if (decision.Approved)
            {
                loanRequestRespons = await _loanRequestRepository.CreateAsync(userId,
                    new LoanApplicationItem(decision.LoanId, request.Type, request.Amount, request.Duration),
                    cancellationToken);
            }
            else
            {
                loanRequestRespons = await _rejectRepository.CreateAsync(userId, 
                    new LoanApplicationItem(decision.LoanId, request.Type, request.Amount, request.Duration), 
                    cancellationToken);
            }

            await SaveLoanDecision(userId, loanRequestRespons.Id, decision.Approved, decision.Decisions, cancellationToken);

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

        public async Task<LoanApplicationRespons> UpdateAsync(string userId, LoanApplicationUpdateRequest request, CancellationToken cancellationToken)
        {
            LoanApplicationItem item = await _loanRequestRepository.GetByLoanIdAsync(userId, request.Id, cancellationToken);

            if(item == null)
            {
                return new LoanApplicationRespons(null, false, new List<string> { "The user does not exist." });
            }

            var decision = await _decisionService.ApplayLoanAsync(
                new LoanApplicationCreateItem(userId, item.Type, request.Amount ?? item.Amount, request.Duration ?? item.Duration), cancellationToken);

            LoanRequestRespons loanRequestRespons = null;

            if (decision.Approved)
            {
                loanRequestRespons = await _loanRequestRepository.UpdateAsync(item, cancellationToken);
            }
            else
            {
                loanRequestRespons = await _rejectRepository.CreateAsync(userId, item, cancellationToken);
            }


            await SaveLoanDecision(userId, item.Id, decision.Approved, decision.Decisions, cancellationToken);

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

        
        private async Task SaveLoanDecision(string userId, string id, bool isApproved, List<string> decision, CancellationToken cancellationToken)
        {
            await _decisionRepository.CreateAsync(new DecisionItem( userId, id, DateTime.UtcNow, isApproved, decision), cancellationToken);
        }
    }
}
