using LoanManager.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LoanManager.Services
{
    public interface ILoanRequestService
    {
        Task<LoanApplicationRespons> CreateAsync(LoanApplicationCreateRequest item, CancellationToken cancellationToken);
        Task<LoanApplicationRespons> UpdateAsync(LoanApplicationUpdateRequest request, CancellationToken cancellationToken);
        Task<LoanApplicationRespons> DeleteAsync(string LoanId, CancellationToken cancellationToken);
        Task<List<LoanApplicationItem>> GetByUserIdAsync(string userId, CancellationToken cancellationToken);
        Task<LoanApplicationItem> GetByLoanIdAsync(string loanId, CancellationToken cancellationToken);
    }
}