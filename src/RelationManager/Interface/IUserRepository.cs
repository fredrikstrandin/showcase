using RelationManager.Models;

namespace RelationManager.Interface
{
    public interface IUserRepository
    {
        Task AddAsync(UserAddItem item);
    }
}