using Grpc.Core;
using Microsoft.Extensions.Logging;
using Northstar.Message;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserManager.Interfaces;
using UserManager.Model;

namespace UserManager.GrpcService;

public class UserGrpcServer : UserGrpcService.UserGrpcServiceBase
{
    private readonly ILogger<UserGrpcServer> _logger;
    private readonly IUserService _userService;
    private readonly IMessageService _messageService;
    public readonly IKeyService _keyService;


    public UserGrpcServer(ILogger<UserGrpcServer> logger, IUserService userService, IMessageService messageService, IKeyService keyService)
    {
        _logger = logger;
        _userService = userService;
        _messageService = messageService;
        _keyService = keyService;
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

        UserRespons ret = await _userService.CreateAsync(userRequest);

        _logger.LogInformation("User request from {Id} has has ended.", request.Id);

        if (ret.Error == null)
        {
            _keyService.AddEmail(request.Email);

            await _messageService.SendNewUserAsync(userRequest);

            return new UserCreateResponsMessage() { Httpstatus = 201, Success = new UserCreateSuccessMessage() { Id = ret.Id } };
        }
        else
        {
            var error = new UserErrorMessage();

            error.Message.Add("Internal Error");
            
            return new UserCreateResponsMessage() { Httpstatus = 500, Error = error };
        }
    }

    public override async Task<UserStreamResponsMessage> CreateStream(IAsyncStreamReader<UserCreateRequestMessage> requestStream, ServerCallContext context)
    {
        int savingAmount = 5000;

        List<UserCreateRequest> userCreateRequest = new List<UserCreateRequest>(savingAmount);

        int count = 0;
        while(await requestStream.MoveNext())
        {
            count++;

            var request = requestStream.Current;

            _logger.LogInformation("User request from {Id} has started.", request.Id);

            if(_keyService.ContainsEmail(request.Email))
            {
                _logger.LogWarning("User {Id} with {Email} allready exist.", request.Id, request.Email);

                continue;
            }
            var userRequest = new UserCreateRequest(
                    request.Id,
                    request.FirstName,
                    request.LastName,
                    request.Email,
                    request.Street,
                    request.Zip,
                    request.City);

            await _messageService.SendNewUserAsync(userRequest);
            
            userCreateRequest.Add(userRequest);

            if ((count % savingAmount) == 0)
            {
                await _userService.CreateManyAsync(userCreateRequest);

                foreach (var user in userCreateRequest)
                {
                    _keyService.AddEmail(user.Email);
                }

                userCreateRequest = new List<UserCreateRequest>(savingAmount);
            }
        }

        return new UserStreamResponsMessage() { Count = count };
    }
    public override async Task<UserGetResponsMessage> Get(UserGetRequestMessage request, ServerCallContext context)
    {
        UserItem item = await _userService.GetByIdAsync(request.Id);

        if(item == null)
        {
            var error = new UserErrorMessage();
            error.Message.Add("Not Found");

            return new UserGetResponsMessage() { Httpstatus = 404,  Error = error};
        }

        return new UserGetResponsMessage() { Httpstatus = 200, Success = new UserGetSuccessMessage() { Id = item.Id, FirstName = item.FirstName, LastName = item.LastName, Email = item.Email, City = item.City, Street = item.Street, Zip = item.Zip } };

    }
}
