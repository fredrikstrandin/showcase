using CustomerManager.Model;
using CustomerManager.Services;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Northstar;
using Northstar.Message;
using System.Threading.Tasks;

namespace CustomerManager.GrpcService;

public class CustomerGrpcServer : CustomerGrpcService.CustomerGrpcServiceBase
{
    private readonly ILogger<CustomerGrpcServer> _logger;
    private readonly ICustomerService _customerService;
    private readonly IMessageService _messageService;

    public CustomerGrpcServer(ILogger<CustomerGrpcServer> logger, ICustomerService customerService, IMessageService messageService)
    {
        _logger = logger;
        _customerService = customerService;
        _messageService = messageService;
    }

    public override async Task<CustomerResponsMessage> Create(CustomerRequestMessage request, ServerCallContext context)
    {
        _logger.LogInformation("Customer request from {Id} has started.", request.Id);

        var customerRequest = new CustomerCreateRequest(
                request.Id,
                request.FirstName,
                request.LastName,
                request.Email,
                request.Street,
                request.Zip,
                request.City);
            
        CustomerRespons ret = await _customerService.CreateAsync(
            customerRequest,
            context.CancellationToken);

        _logger.LogInformation("Customer request from {Id} has has ended.", request.Id);

        if (ret.Error == null)
        {
            await _messageService.SendNewUserAsync(customerRequest);

            return new CustomerResponsMessage() { Success = new UserCreateSuccessMessage() { Id = ret.Id } };
        }
        else
        {
            var error = new UserErrorMessage();

            error.Message.Add(ret.Error.Message);
            while(ret.Error.InnerException != null)
            {
                error.Message.Add(ret.Error.Message);
            }

            return new CustomerResponsMessage() { Error = error };
        }
    }
}
