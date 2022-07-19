using IdentityManager.Models;
using System.Security.Claims;

namespace IdentityManager.Interface;

public interface IUserStoreRepository
{
    Task<UserItem> FindByExternalProviderAsync(string provider, string userId);
    Task<UserItem> FindBySubjectIdAsync(string subjectId);
    Task<UserItem> FindByUsernameAsync(string username);
    Task<(string Hash, byte[] Salt)> FindHashPasswordAndSaltByUsernameAsync(string username);
    Task<UserItem> AddProvisionUserAsync(string name, string provider, string userId, ICollection<Claim> claims);
    Task<bool> IsActive(string username);
    Task<LoginRespons> CreateUserAsync(string email, string hash, byte[] salt, List<Claim> claims);
}
