using CustomerManager.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CustomerManager.Services
{
    public interface ICustomerService
    {
        Task<CustomerRespons> CreateAsync(CustomerCreateRequest reques, CancellationToken cancellationTokent);
        Task<CustomerRespons> UpdateAsync(UserUpdateRequest reques, CancellationToken cancellationTokent);
        Task<UserItem> GetByIdAsync(string userId, CancellationToken cancellationToken);
        Task<List<UserItem>> GetAsync(CancellationToken cancellationToken);
    }
}