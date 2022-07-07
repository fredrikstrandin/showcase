using Grpc.Core;
using Northstar.Message;
using NorthStarGraphQL.Interface;
using NorthStarGraphQL.Models;

namespace NorthStarGraphQL.Repository;

public class CustomerRepository : ICustomerRepository
{
    readonly private ILogger<CustomerRepository> _logger;
    private readonly CustomerGrpcService.CustomerGrpcServiceClient _client;

    public CustomerRepository(CustomerGrpcService.CustomerGrpcServiceClient client, ILogger<CustomerRepository> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<(string id, string error)> CreateAsync(CustomerCreateItem item, CancellationToken cancellationToken)
    {
        try
        {
            CustomerRequestMessage req = new CustomerRequestMessage()
            {
                Id = item.Id,
                FirstName = item.FirstName,
                LastName = item.LastName,
                Email = item.Email,
                Street = item.Street,
                City = item.City,
                Zip = item.Zip,
                MonthlyIncome = item.MonthlyIncome ?? 0
            };

            var reply = await _client.CreateAsync(req, cancellationToken: cancellationToken);

            if (reply.MessageTypeCase == CustomerResponsMessage.MessageTypeOneofCase.Success)
            {
                return new(reply.Success.Id, null);
            }

            if (reply.MessageTypeCase == CustomerResponsMessage.MessageTypeOneofCase.Error)
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
            _logger.LogCritical(e, "Rpc faild: from GrapQL to customermagnager");

            throw;
        }
    }
}
