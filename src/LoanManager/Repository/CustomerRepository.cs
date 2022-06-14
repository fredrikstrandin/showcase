using CustomerManager.RPC;
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
        public async Task<int> GetMonthlyIncomeAsync(string id, CancellationToken cancellationToken)
        {
            using (GrpcChannel channel = GrpcChannel.ForAddress("https://localhost:5002"))
            {
                var client = new CustomerRpc.CustomerRpcClient(channel);

                try
                {
                    MonthlyIncomeRequest req = new MonthlyIncomeRequest()
                    {
                        Id = id
                    };

                    var reply = await client.GetMonthlyIncomeAsync(req, cancellationToken: cancellationToken);

                    return reply.MonthlyIncome;
                }
                catch (RpcException)
                {
                    return 0;
                }
            }
        }
    }
}
