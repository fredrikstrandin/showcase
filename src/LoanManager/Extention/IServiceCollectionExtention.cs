using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Northstar.Message;
using System;
using System.Net.Http;

namespace LoanManager.Extention
{
    public static class IServiceCollectionExtention
    {
        public static IHttpClientBuilder AddClients(this IServiceCollection services, ConfigurationManager config)
        {
            

            return services.AddGrpcClient<CustomerGrpcService.CustomerGrpcServiceClient>(
                o => { o.Address = new Uri(config["CustomerManager:ServerUrl"]); }
            )
                .ConfigureChannel(x =>
                {
                    if (config.GetValue<bool>("CustomerManager:AllowInsecureConnections"))
                    {
                        var httpHandler = new HttpClientHandler();

                        // Return `true` to allow certificates that are untrusted/invalid
                        httpHandler.ServerCertificateCustomValidationCallback =
                            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

                        x.HttpHandler = httpHandler;
                    }                    
                });
        }
    }
}
