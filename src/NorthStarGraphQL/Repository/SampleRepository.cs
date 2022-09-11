using CommonLib.Models;
using Google.Protobuf.Collections;
using Grpc.Core;
using Northstar.Message;
using NorthStarGraphQL.Interface;
using NorthStarGraphQL.Models;
using System.Net;

namespace NorthStarGraphQL.Repository;

public class SampleGRPCRepository : ISampleRepository
{
    private readonly ILogger<SampleGRPCRepository> _logger;
    private readonly UserGrpcService.UserGrpcServiceClient _client;

    public SampleGRPCRepository(UserGrpcService.UserGrpcServiceClient client, ILogger<SampleGRPCRepository> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<ErrorItem> CreateSampleUserAsync(int count)
    {
        try
        {
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
}
