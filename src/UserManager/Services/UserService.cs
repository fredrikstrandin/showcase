using IdentityModel;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using UserManager.Interfaces;
using UserManager.Model;

namespace UserManager.Services;

public class UserService : IUserService
{
    public readonly IUserRepository _userRepository;
    public readonly IPasswordService _passwordService;

    public UserService(IUserRepository userRepository, IPasswordService passwordService)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
    }

    public async Task<UserRespons> CreateAsync(UserCreateRequest request)
    {
        List<Claim> claims = new List<Claim>() { new Claim(JwtClaimTypes.Name, request.FirstName) };

        UserItem item = new UserItem(
            request.Id,
            request.FirstName,
            request.LastName,
            request.Email,
            request.Street,
            request.Zip,
            request.City);

        var ret = await _userRepository.CreateAsync(item);

        return ret;
    }

    public async Task<UserRespons> UpdateAsync(UserUpdateRequest request)
    {
        return await _userRepository.UpdateAsync(request);
    }

    public Task<List<UserItem>> GetAsync()
    {
        return _userRepository.GetAsync();
    }

    public async Task<UserItem> GetByIdAsync(string userId)
    {
        return await _userRepository.GetByIdAsync(userId);
    }
}
