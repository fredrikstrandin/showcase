using Northstar.Message;
using Grpc.Core;
using Grpc.Net.Client;
using LoanManager.Interface;
using LoanManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace LoanManager.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerGrpcService.CustomerGrpcServiceClient _client;

        public CustomerRepository(CustomerGrpcService.CustomerGrpcServiceClient client)
        {
            _client = client;
        }

        public async Task<int?> GetMonthlyIncomeAsync(string id, CancellationToken cancellationToken)
        {
            try
            {
                MonthlyIncomeRequest req = new MonthlyIncomeRequest()
                {
                    Id = id
                };

                var reply = await _client.GetMonthlyIncomeAsync(req, cancellationToken: cancellationToken);

                return reply.MonthlyIncome;
            }
            catch (RpcException)
            {
                return null;
            }
        }
    }
}
