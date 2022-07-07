using CustomerManager.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CustomerManager.Services
{
    public interface ICustomerService
    {
        Task<CustomerRespons> CreateAsync(CustomerCreateRequest reques, CancellationToken cancellationTokent);
        Task<CustomerRespons> UpdateAsync(CustomerUpdateRequest reques, CancellationToken cancellationTokent);
        Task<CustomerItem> GetByIdAsync(string userId, CancellationToken cancellationToken);
        Task<List<CustomerItem>> GetAsync(CancellationToken cancellationToken);
        Task<int> GetMonthlyIncomeAsync(string Id, CancellationToken cancellationToken);
    }
}