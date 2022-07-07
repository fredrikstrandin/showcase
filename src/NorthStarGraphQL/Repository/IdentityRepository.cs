using Google.Protobuf;
using Grpc.Core;
using Microsoft.Extensions.Options;
using Northstar.Message;
using NorthStarGraphQL.Interface;
using NorthStarGraphQL.Models;

namespace NorthStarGraphQL.Repository;

public class IdentityRepository : IIdentityRepository
{
    private readonly ILogger<IdentityRepository> _logger;
    private readonly IdentityGrpcService.IdentityGrpcServiceClient _client;

    public IdentityRepository(ILogger<IdentityRepository> logger, IdentityGrpcService.IdentityGrpcServiceClient client)
    {
        _logger = logger;
        _client = client;
    }

    public async Task<(string id, string error)> CreateLoginAsync(LoginCreateItem item)
    {
        LoginMessage message = new LoginMessage()
        {
            Nickname = item.Nickname,
            Email = item.Email,
            Password = item.Password
        };

        foreach (var claim in item.Claims)
        {
            message.Claims.Add(new ClaimMessage() { Type = claim.Type, Value = claim.Value });
        }
        try
        {
            var reply = await _client.CreateAsync(message);

            if (reply.MessageTypeCase == LoginResponsMessage.MessageTypeOneofCase.Success)
            {
                return new(reply.Success.Id, null);
            }

            if (reply.MessageTypeCase == LoginResponsMessage.MessageTypeOneofCase.Error)
            {
                string error = string.Empty;

                foreach (var errorMessage in reply.Error.Message)
                {
                    error += errorMessage;
                }

                _logger.LogInformation(error);

                return new(null, error);
            }

            return new(null, "Unknown");
        }
        catch (RpcException e)
        {
            _logger.LogCritical(e, "Rpc faild: from GrapQL to identitymagnager");

            throw;
        }
    }
}
