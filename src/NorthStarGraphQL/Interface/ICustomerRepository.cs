using NorthStarGraphQL.Models;

namespace NorthStarGraphQL.Interface;

public interface ICustomerRepository
{
    Task<(string id, string error)> CreateAsync(UserCreateItem item, CancellationToken cancellationToken);
}
