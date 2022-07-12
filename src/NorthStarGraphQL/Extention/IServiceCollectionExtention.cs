using Northstar.Message;

namespace NorthStarGraphQL.Extention;

public static class IServiceCollectionExtention
{
    public static IHttpClientBuilder AddClients(this IServiceCollection services, ConfigurationManager config)
    {
        services.AddGrpcClient<CustomerGrpcService.CustomerGrpcServiceClient>(
            o => { 
                o.Address = new Uri(config["CustomerManager:ServerUrl"]); 
            }
        );

        return services.AddGrpcClient<IdentityGrpcService.IdentityGrpcServiceClient>(
            o => { 
                o.Address = new Uri(config["IdentityManager:ServerUrl"]); 
            }
        );
    }
}
