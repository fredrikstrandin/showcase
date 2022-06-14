using Google.Protobuf.Collections;
using IdentityManager.Models;
using System.Security.Claims;

namespace IdentityManager.Interface;

public interface IUserStoreService
{
    Task<UserItem> AutoProvisionUserAsync(string provider, string userId, List<Claim> claims);
    Task<UserItem> FindByExternalProviderAsync(string provider, string userId);
    Task<UserItem> FindBySubjectIdAsync(string subjectId);
    Task<UserItem> FindByUserNameAsync(string username);
    Task<bool> ValidateCredentialsAsync(string username, string password);
    Task<bool> IsActiveAsync(string username);
    Task<string> AddUserAsync(string nickname, string password, byte[] salt, string sub, List<Claim> claims);
}
