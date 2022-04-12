using CustomerManager.Model;
using LoanManager.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LoanManager.Repository
{
    public interface ILoanRequestRepository
    {
        Task<LoanApplicationItem> GetByLoanIdAsync(string loanId, CancellationToken cancellationToken);
        Task<List<LoanApplicationItem>> GetByUserIdAsync(string userId, CancellationToken cancellationToken);
        Task<LoanRequestRespons> CreateAsync(LoanApplicationCreateRequest request, CancellationToken cancellationToken);
        Task<LoanRequestRespons> UpdateAsync(LoanApplicationItem item, CancellationToken cancellationToken);
        Task<LoanRequestRespons> DeleteAsync(string loanId, CancellationToken cancellationToken);        
    }
}