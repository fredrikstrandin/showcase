using NorthStarGraphQL.GraphQL.Types.User;
using System.Security.Claims;

namespace NorthStarGraphQL.GraphQL;

public class NorthStarQuery
{
    public UserType GetMe(ClaimsPrincipal claims)
    {
        var c = claims.FindFirstValue("sub");
        if (c == null)
            return null;

        return new UserType("1", "Lars", "Larsson", "fre@svt.se", "sveav 34", "112 11", "Stockholm");
    }

}