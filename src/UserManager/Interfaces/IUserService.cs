using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UserManager.Model;

namespace UserManager.Interfaces
{
    public interface IUserService
    {
        Task<UserRespons> CreateAsync(UserCreateRequest reques);
        Task<UserRespons> UpdateAsync(UserUpdateRequest reques);
        Task<UserItem> GetByIdAsync(string userId);
        Task<List<UserItem>> GetAsync();
    }
}