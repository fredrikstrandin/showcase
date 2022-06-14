using CustomerManager.Model;
using LoanManager.Model;
using System.Threading;
using System.Threading.Tasks;

namespace LoanManager.Interface;

public interface IRejectRepository
{
    Task<LoanRequestRespons> CreateAsync(LoanApplicationItem item, CancellationToken cancellationToken);
}