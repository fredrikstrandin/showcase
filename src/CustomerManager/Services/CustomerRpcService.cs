using CustomerManager.Interfaces;
using CustomerManager.RPC;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CustomerManager.Services
{
    //Denna klass kam man diskutera om det skall vara en Service eller n�got annat och bara ropa p� 
    //ett tj�nstelager. Men f�r att inte denna uppgift skall springa iv�g l�ter jag den vara en tj�nst.
    public class CustomerRpcService : CustomerRpc.CustomerRpcBase
    {
        private readonly ILogger<CustomerRpcService> _logger;
        private readonly ICustomerRepository _customerRepository;

        public CustomerRpcService(ILogger<CustomerRpcService> logger, ICustomerRepository customerRepository)
        {
            _logger = logger;
            _customerRepository = customerRepository;
        }

        public override async Task<SaleryReply> GetMonthlyIncome(SaleryRequest request, ServerCallContext context)
        {
            int monthlyIncome = await _customerRepository.GetMonthlyIncomeAsync(request.Id, context.CancellationToken);

            return new SaleryReply() { MonthlyIncome = monthlyIncome };
        }
    }
}
