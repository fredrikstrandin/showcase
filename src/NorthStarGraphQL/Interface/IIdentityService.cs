using NorthStarGraphQL.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NorthStarGraphQL.Interface;

public interface IIdentityService
{
    Task<(string id, string error)> CreateLoginAsync(LoginCreateItem item);
}