using RelationManager.Interface;
using RelationManager.Models;

namespace RelationManager.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task AddAsync(UserAddItem item)
        {
            await _userRepository.AddAsync(item);
        }
    }
}
