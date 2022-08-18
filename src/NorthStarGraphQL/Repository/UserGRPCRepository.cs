using CommonLib.Models;
using Google.Protobuf.Collections;
using Grpc.Core;
using Northstar.Message;
using NorthStarGraphQL.Interface;
using NorthStarGraphQL.Models;
using System.Net;

namespace NorthStarGraphQL.Repository;

public class UserGRPCRepository : IUserRepository
{
    private readonly ILogger<UserGRPCRepository> _logger;
    private readonly UserGrpcService.UserGrpcServiceClient _client;

    public UserGRPCRepository(UserGrpcService.UserGrpcServiceClient client, ILogger<UserGRPCRepository> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<(string id, ErrorItem error)> CreateAsync(UserCreateItem item)
    {
        try
        {
            UserCreateRequestMessage req = new UserCreateRequestMessage()
            {
                Id = item.Id,
                FirstName = item.FirstName,
                LastName = item.LastName,
                Email = item.Email,
                Street = item.Street,
                City = item.City,
                Zip = item.Zip
            };

            var reply = await _client.CreateAsync(req);

            if (reply.Httpstatus == (int)HttpStatusCode.Created && reply.Success != null)
            {
                return new(reply.Success.Id, null);
            }
            else if (reply.Error != null)
            {
                string error = string.Empty;

                foreach (var errorMessage in reply.Error.Message)
                {
                    error += errorMessage;
                }

                _logger.LogInformation(error);

                return new(null, new ErrorItem(error, reply.Httpstatus));
            }

            return new(null, new ErrorItem("Unknown", reply.Httpstatus));
        }
        catch (RpcException e)
        {
            _logger.LogCritical(e, "Rpc faild: from GrapQL to usermagnager");

            throw;
        }
    }

    public async Task<(UserItem item, ErrorItem error)> GetAsync(string userId)
    {
        try
        {
            UserGetRequestMessage req = new UserGetRequestMessage()
            {
                Id = userId
            };

            UserGetResponsMessage reply = await _client.GetAsync(req);

            if (reply.Httpstatus == (int)HttpStatusCode.OK && reply.Success != null)
            {
                return new(new UserItem(reply.Success.Id, reply.Success.FirstName, reply.Success.LastName, reply.Success.Email, reply.Success.Street, reply.Success.Zip, reply.Success.City), null);
            }
            else if (reply.MessageTypeCase == UserGetResponsMessage.MessageTypeOneofCase.Error)
            {
                string error = string.Empty;

                foreach (var errorMessage in reply?.Error?.Message ?? new RepeatedField<string>())
                {
                    error += errorMessage;
                }

                _logger.LogInformation(error);

                return new(null, new ErrorItem(error, reply.Httpstatus));
            }

            return new(null, new ErrorItem("Unknown", reply.Httpstatus));
        }
        catch (RpcException e)
        {
            _logger.LogCritical(e, "Rpc faild: from GrapQL to usermagnager");

            return  new(null, ErrorItem.InternalError());
        }
    }
}
