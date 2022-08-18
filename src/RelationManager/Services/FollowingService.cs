using RelationManager.Interface;
using RelationManager.Models;

namespace RelationManager.Services
{
    public class FollowingService : IFollowingService
    {
        private readonly IFollowingRepository _followingRepository;

        public FollowingService(IFollowingRepository followingRepository)
        {
            _followingRepository = followingRepository;
        }

        public async Task CreateAsync(FollowingCreateRequest reqest)
        {
            await _followingRepository.CreateAsync(reqest);
        }
    }
}
