using Grpc.Core;
using Northstar.Message;
using RelationManager.Interface;
using RelationManager.Models;

namespace UserManager.GrpcService;

public class FollowingGrpcServer : FollowingGrpcService.FollowingGrpcServiceBase
{
    private readonly ILogger<FollowingGrpcServer> _logger;
    private readonly IFollowingService _followingService;

    public FollowingGrpcServer(ILogger<FollowingGrpcServer> logger, IFollowingService followingService)
    {
        _logger = logger;
        _followingService = followingService;
    }

    public override async Task<FollowingCreateResponsMessage> Create(FollowingCreateRequestMessage request, ServerCallContext context)
    {
        _logger.LogInformation("User request from {Id} has started.", request.UserId);

        var followingRequest = new FollowingCreateRequest(
                request.UserId,
                request.FollowingId);

        await _followingService.CreateAsync(followingRequest);

        
        _logger.LogInformation("User request from {Id} has has ended.", request.UserId);

        return new FollowingCreateResponsMessage() { Httpstatus = 201, Success = new FollowingCreateSuccessMessage() { UserId = request.UserId } };        
    }
}
