using CustomerManager.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CustomerManager.Interfaces;

public interface ICustomerRepository
{
    Task<List<UserItem>> GetAsync(CancellationToken cancellationToken);
    Task<UserItem> GetByIdAsync(string userId, CancellationToken cancellationToken);
    Task<CustomerRespons> UpdateAsync(UserUpdateRequest request, CancellationToken cancellationToken);
    Task<CustomerRespons> CreateAsync(UserItem item, CancellationToken cancellationToken);
}
