using LoanManager.Model;
using System.Threading;
using System.Threading.Tasks;

namespace LoanManager.Interface;

public interface ICustomerRepository
{
    Task<int?> GetMonthlyIncomeAsync(string id, CancellationToken cancellationToken);
}
