using CommonLib.Extensions;
using HotChocolate.AspNetCore.Authorization;
using NorthStarGraphQL.GraphQL.Types.User;
using System.Security.Claims;

namespace NorthStarGraphQL.GraphQL;

public class NorthStarQuery
{
    [Authorize]
    public UserType GetMe(ClaimsPrincipal claims)
    {
        string c = claims.GetSub();
        if (c == null)
            return null;

        return new UserType(c, "Lars", "Larsson", "fre@svt.se", "sveav 34", "112 11", "Stockholm");
    }

}