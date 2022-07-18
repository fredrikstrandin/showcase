using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UserManager.Model;

namespace UserManager.Interfaces;

public interface IUserRepository
{
    Task<List<UserItem>> GetAsync();
    Task<UserItem> GetByIdAsync(string userId);
    Task<UserRespons> UpdateAsync(UserUpdateRequest request);
    Task<UserRespons> CreateAsync(UserItem item);
}
