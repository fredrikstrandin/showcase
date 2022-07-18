using CommonLib.Models;
using NorthStarGraphQL.Interface;
using NorthStarGraphQL.Models;

namespace NorthStarGraphQL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<(string id, ErrorItem error)> CreateAsync(UserCreateItem item)
        {
            return  await _userRepository.CreateAsync(item);
            
        }

        public async Task<(UserItem item, ErrorItem error)> GetAsync(string userId)
        {
            return await _userRepository.GetAsync(userId);
        }
    }
}
