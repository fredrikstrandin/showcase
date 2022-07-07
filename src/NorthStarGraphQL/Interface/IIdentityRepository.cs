using Google.Protobuf;
using Northstar.Message;
using NorthStarGraphQL.Models;
using System.Threading.Tasks;

namespace NorthStarGraphQL.Interface;

public interface IIdentityRepository
{
    Task<(string id, string error)> CreateLoginAsync(LoginCreateItem message);
}
