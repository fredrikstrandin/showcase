using LoanManager.Model;
using System.Threading;
using System.Threading.Tasks;

namespace LoanManager.Interface;

public interface IDecisionService
{
    Task<DecisionItem> ApplayLoanAsync(LoanApplicationItem item, CancellationToken cancellationToken);
}
