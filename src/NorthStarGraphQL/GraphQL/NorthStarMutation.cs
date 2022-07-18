using HotChocolate.AspNetCore.Authorization;
using NorthStarGraphQL.GraphQL.Types.User;
using NorthStarGraphQL.Interface;
using NorthStarGraphQL.Models;
using System.Security.Claims;

namespace NorthStarGraphQL.GraphQL;

public class NorthStarMutation
{
    readonly private ICustomerRepository _customerRepository;
    readonly private IIdentityService _identityService;
    readonly private ILogger<NorthStarMutation> _logger;

    public NorthStarMutation(ICustomerRepository customerRepository, IIdentityService identityService, ILogger<NorthStarMutation> logger)
    {
        _customerRepository = customerRepository;
        _identityService = identityService;
        _logger = logger;
    }

    [Authorize]
    public async Task<UserType> UpdateUserAsync(string test, ClaimsPrincipal claims)
    {
        _logger.LogDebug(test);

        foreach (var item in claims.Claims)
        {
            _logger.LogDebug($"{item.Type}: {item.Value}");
        }

        return UserType.Default();
    }

    public async Task<UserType> CreateUserAsync(string firstName, string lastName, string email, string nickname, string password, string street, string zip, string city)
    {
        LoginCreateItem loginItem = new LoginCreateItem(
                nickname,
                email,
                password,
                new List<ClaimItem>() { new ClaimItem("userType", "member") });

        var login = await _identityService.CreateLoginAsync(loginItem);

        if (login.error == null)
        {
            UserCreateItem customerItem = new UserCreateItem(
                login.id,
                firstName,
                lastName,
                email,
                street,
                zip,
                city);

            var ret = await _customerRepository.CreateAsync(customerItem, CancellationToken.None);

            if (string.IsNullOrWhiteSpace(ret.error))
            {
                if (ret.id != null)
                {
                    return new UserType(ret.id,
                        customerItem.FirstName,
                        customerItem.LastName,
                        customerItem.Email,
                        customerItem.Street,
                        customerItem.Zip,
                        customerItem.City);
                }
            }
            else
            {
                throw new Exception(ret.error);
            }
        }
        else
        {
            throw new Exception(login.error);
        }

        return null;
    }

}