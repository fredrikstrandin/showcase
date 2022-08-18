using CommonLib.Validator;
using Grpc.Net.Client;
using Northstar.Message;
using System.Security.Cryptography.X509Certificates;

namespace GeneratorWeb.Extention;

public static class IServiceCollectionExtention
{
    public static IHttpClientBuilder AddClients(this IServiceCollection services, ConfigurationManager config)
    {
        services.AddGrpcClient<FollowingGrpcService.FollowingGrpcServiceClient>(
            o =>
            {
                o.Address = new Uri(config["RelationManager:ServerUrl"]);
            }
        )
        .ConfigureChannel(x =>
        {
            if (config.GetValue<bool>("RelationManager:AllowInsecureConnections"))
            {
                var httpHandler = new HttpClientHandler();

                // Return `true` to allow certificates that are untrusted/invalid
                httpHandler.ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

                x.HttpHandler = httpHandler;
            }
        });

        return services.AddGrpcClient<UserGrpcService.UserGrpcServiceClient>(
            o =>
            {
                o.Address = new Uri(config["UserManager:ServerUrl"]);
            }
        )
        .ConfigureChannel(x =>
        {
            if (config.GetValue<bool>("UserManager:AllowInsecureConnections"))
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
