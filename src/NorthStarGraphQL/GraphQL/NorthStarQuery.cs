using CommonLib.Extensions;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Execution;
using NorthStarGraphQL.GraphQL.Types.User;
using NorthStarGraphQL.Interface;
using System.Security.Claims;

namespace NorthStarGraphQL.GraphQL;

public class NorthStarQuery
{
    private readonly IUserService _userService;

    public NorthStarQuery(IUserService userService)
    {
        _userService = userService;
    }

    [Authorize]
    public async Task<UserType> GetMeAsync(ClaimsPrincipal claims)
    {
        string userId = claims.GetSub();
        if (userId == null)
        {
            throw new QueryException(new Error("Jwt has no sub", "NO_AUTH"));
        }

        var ret = await _userService.GetAsync(userId);

        if(ret.error != null)
        {
            throw new QueryException(ret.error.Create());
        }

        return new UserType(ret.item.Id, ret.item.FirstName, ret.item.LastName, ret.item.Email, ret.item.Street, ret.item.Zip, ret.item.City);
    }

}