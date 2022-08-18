using CommonLib.Exceptions;
using IdentityModel;
using Microsoft.Extensions.Logging;
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
    public readonly IKeyService _keyService;
    public readonly ILogger<UserService> _logger;

    public UserService(IUserRepository userRepository, IPasswordService passwordService, IKeyService keyService, ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _keyService = keyService;
        _logger = logger;
    }

    public async Task<UserRespons> CreateAsync(UserCreateRequest request)
    {        
        if(_keyService.ContainsEmail(request.Email))
        {
            _logger.LogWarning("User {Id} with {Email} allready exist.", request.Id, request.Email);

            return new UserRespons(null, new DuplicateException($"User {request.Id} with {request.Email} allready exist."));
        }

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

    public async Task CreateManyAsync(List<UserCreateRequest> requests)
    {
        List<UserItem> users = new List<UserItem>();

        foreach (var request in requests)
        {
            if (_keyService.ContainsEmail(request.Email))
            {
                _logger.LogWarning("User {Id} with {Email} allready exist.", request.Id, request.Email);

                continue;
            }

            users.Add(new UserItem(
                request.Id,
                request.FirstName,
                request.LastName,
                request.Email,
                request.Street,
                request.Zip,
                request.City));
        }

        await _userRepository.CreateManyAsync(users);
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
