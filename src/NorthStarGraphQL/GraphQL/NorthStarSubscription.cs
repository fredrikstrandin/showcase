using HotChocolate.AspNetCore.Authorization;
using NorthStarGraphQL.GraphQL.Types.User;
using NorthStarGraphQL.Models;
using System.Security.Claims;

namespace NorthStarGraphQL.GraphQL;

public class NorthStarSubscription
{
    [Subscribe]
    public UserCreateType NewUserAdded([EventMessage] UserCreateItem item, ClaimsPrincipal claims)
    {
        var c = claims.FindFirstValue("Sub");
        if (c == null)
            return null;

        return new UserCreateType(item.Id, item.FirstName, item.LastName, item.Email, item.Street, item.Zip, item.City);
    }
}