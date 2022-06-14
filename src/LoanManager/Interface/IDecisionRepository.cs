using LoanManager.Model;
using System.Threading;
using System.Threading.Tasks;

namespace LoanManager.Interface;

public interface IDecisionRepository
{
    Task<bool> CreateAsync(DecisionItem item, CancellationToken cancellationToken);
}
