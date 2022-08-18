using Grpc.Core;
using Northstar.Message;
using SampleManager.Interface;
using UserManager.Model;

namespace UserManager.GrpcService;

public class SampleGrpcServer : SampleGrpcService.SampleGrpcServiceBase
{
    private readonly ILogger<SampleGrpcServer> _logger;
    private readonly ISampleService _sampleService;

    public SampleGrpcServer(ILogger<SampleGrpcServer> logger, ISampleService sampleService)
    {
        _logger = logger;
        _sampleService = sampleService;
    }

    public override async Task<SampleCreateUsersResponsMessage> Create(SampleCreateUsersRequestMessage request, ServerCallContext context)
    {        
        _logger.LogInformation($"Creating {request.Count} sample users.");

        UsersRespons ret = await _sampleService.CreateUsersAsync(request.Count);

        
        if (ret.Error == null)
        {
            _logger.LogInformation($"User samples was created.");

            return new SampleCreateUsersResponsMessage() { Httpstatus = 201 };
        }
        else
        {
            var error = new SampleErrorMessage();

            error.Message.Add("Internal Error");

            return new SampleCreateUsersResponsMessage() { Httpstatus = 500, Error = error };
        }
    }
}