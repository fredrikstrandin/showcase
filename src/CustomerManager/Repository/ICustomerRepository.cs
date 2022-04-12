using CustomerManager.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CustomerManager.Repository
{
    public interface ICustomerRepository
    {
        Task<int> GetMonthlyIncomeAsync(string id, CancellationToken cancellationToken);
        Task<List<CustomerItem>> GetAsync(CancellationToken cancellationToken);
        Task<CustomerItem> GetByIdAsync(string userId, CancellationToken cancellationToken);
        Task<CustomerRespons> UpdateAsync(CustomerUpdateRequest request, CancellationToken cancellationToken);
        Task<CustomerRespons> CreateAsync(CustomerCreateItem item, CancellationToken cancellationToken);
    }
}