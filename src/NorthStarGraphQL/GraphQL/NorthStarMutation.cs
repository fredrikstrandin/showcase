using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Execution;
using NorthStarGraphQL.GraphQL.Types.User;
using NorthStarGraphQL.Interface;
using NorthStarGraphQL.Models;
using CommonLib.Extensions;
using System.Security.Claims;

namespace NorthStarGraphQL.GraphQL;

public class NorthStarMutation
{
    readonly private IUserRepository _userRepository;
    readonly private IIdentityService _identityService;
    readonly private ILogger<NorthStarMutation> _logger;

    public NorthStarMutation(IUserRepository userRepository, IIdentityService identityService, ILogger<NorthStarMutation> logger)
    {
        _userRepository = userRepository;
        _identityService = identityService;
        _logger = logger;
    }

    [Authorize]
    public Task<UserType> UpdateUserAsync(string test, ClaimsPrincipal claims)
    {
        _logger.LogDebug(test);

        foreach (var item in claims.Claims)
        {
            _logger.LogDebug($"{item.Type}: {item.Value}");
        }

        return Task.FromResult(UserType.Default());
    }

    public async Task<UserType> CreateUserAsync(string firstName, string lastName, string email, string password, string street, string zip, string city)
    {
        LoginCreateItem loginItem = new LoginCreateItem(
                email,
                password,
                new List<ClaimItem>() { new ClaimItem("userType", "member") });

        var login = await _identityService.CreateLoginAsync(loginItem);

        if (login.error == null)
        {
            UserCreateItem userItem = new UserCreateItem(
                login.id,
                firstName,
                lastName,
                email,
                street,
                zip,
                city);

            var ret = await _userRepository.CreateAsync(userItem);
            
            if (ret.error != null)
            {
                if (ret.id != null)
                {
                    return new UserType(ret.id,
                        userItem.FirstName,
                        userItem.LastName,
                        userItem.Email,
                        userItem.Street,
                        userItem.Zip,
                        userItem.City);
                }
            }
            else
            {   
                throw new QueryException(ret.error.Create());
            }
        }
        else
        {
            throw new QueryException(login.error);
        }

        return null;
    }

}