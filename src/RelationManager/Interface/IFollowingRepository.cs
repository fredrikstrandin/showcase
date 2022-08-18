using RelationManager.Models;

namespace RelationManager.Interface
{
    public interface IFollowingRepository
    {
        Task CreateAsync(FollowingCreateRequest reqest);
    }
}