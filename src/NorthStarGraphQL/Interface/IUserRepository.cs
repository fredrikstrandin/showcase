using CommonLib.Models;
using NorthStarGraphQL.Models;

namespace NorthStarGraphQL.Interface;

public interface IUserRepository
{
    Task<(string id, ErrorItem error)> CreateAsync(UserCreateItem item);
    Task<(UserItem item, ErrorItem error)> GetAsync(string userId);
}
