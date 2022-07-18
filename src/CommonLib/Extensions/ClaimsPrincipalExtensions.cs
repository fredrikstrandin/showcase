using System.Security.Claims;

namespace CommonLib.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetSub(this ClaimsPrincipal claims)
    {
        var c = claims.FindFirst("sub");

        if (c == null)
        {
            return null;
        }
        
        return c.Value;
    }
}
