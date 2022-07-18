using Grpc.Core;
using Microsoft.Extensions.Logging;
using Northstar.Message;
using System.Threading.Tasks;
using UserManager.Interfaces;
using UserManager.Model;

namespace UserManager.GrpcService;

public class UserGrpcServer : UserGrpcService.UserGrpcServiceBase
{
    private readonly ILogger<UserGrpcServer> _logger;
    private readonly IUserService _userService;
    private readonly IMessageService _messageService;

    public UserGrpcServer(ILogger<UserGrpcServer> logger, IUserService userService, IMessageService messageService)
    {
        _logger = logger;
        _userService = userService;
        _messageService = messageService;
    }

    public override async Task<UserCreateResponsMessage> Create(UserCreateRequestMessage request, ServerCallContext context)
    {
        _logger.LogInformation("User request from {Id} has started.", request.Id);

        var userRequest = new UserCreateRequest(
                request.Id,
                request.FirstName,
                request.LastName,
                request.Email,
                request.Street,
                request.Zip,
                request.City);

        UserRespons ret = await _userService.CreateAsync(
            userRequest);

        _logger.LogInformation("User request from {Id} has has ended.", request.Id);

        if (ret.Error == null)
        {
            await _messageService.SendNewUserAsync(userRequest);

            return new UserCreateResponsMessage() { Success = new UserCreateSuccessMessage() { Id = ret.Id } };
        }
        else
        {
            var error = new UserErrorMessage();

            error.Message.Add(ret.Error.Message);
            while (ret.Error.InnerException != null)
            {
                error.Message.Add(ret.Error.Message);
            }

            return new UserCreateResponsMessage() { Error = error };
        }
    }
}
