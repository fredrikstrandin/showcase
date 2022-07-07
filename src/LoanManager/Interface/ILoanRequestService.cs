using LoanManager.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LoanManager.Interface;

public interface ILoanRequestService
{
    Task<LoanApplicationRespons> CreateAsync(string userId, LoanApplicationCreateRequest item, CancellationToken cancellationToken);
    Task<LoanApplicationRespons> UpdateAsync(string userId, LoanApplicationUpdateRequest request, CancellationToken cancellationToken);
    Task<LoanApplicationRespons> DeleteAsync(string LoanId, CancellationToken cancellationToken);
    Task<List<LoanApplicationItem>> GetByUserIdAsync(string userId, CancellationToken cancellationToken);
    Task<LoanApplicationItem> GetByLoanIdAsync(string userId, string loanId, CancellationToken cancellationToken);
}
