using RelationManager.Models;

namespace RelationManager.Interface;

public interface IUserService
{
    Task AddAsync(UserAddItem item);
}