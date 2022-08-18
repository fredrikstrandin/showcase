using RelationManager.Models;

namespace RelationManager.Interface
{
    public interface IFollowingService
    {
        Task CreateAsync(FollowingCreateRequest reqest);
    }
}