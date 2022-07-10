using HotChocolate.AspNetCore.Authorization;
using NorthStarGraphQL.Interface;
using NorthStarGraphQL.Models;
using System.Security.Claims;

namespace NorthStarGraphQL.GraphQL;

public class NorthStarSubscription
{
    [Subscribe]
    public UserCreateItem NewUserAdded([EventMessage] UserCreateItem item, ClaimsPrincipal claims)
    {
        var c = claims.FindFirstValue("Sub");
        if (c == null)
            return null;

        return item;
    }
}