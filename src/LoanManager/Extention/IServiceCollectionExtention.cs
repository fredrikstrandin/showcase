using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Northstar.Message;
using System;

namespace LoanManager.Extention
{
    public static class IServiceCollectionExtention
    {
        public static IHttpClientBuilder AddClients(this IServiceCollection services, ConfigurationManager config)
        {
            return services.AddGrpcClient<CustomerGrpcService.CustomerGrpcServiceClient>(
                o => { o.Address = new Uri(config["CustomerManager:ServerUrl"]); }
            );
        }
    }
}
