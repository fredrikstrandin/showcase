using CommonLib.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using NorthStarGraphQL.Backgrondservices;
using NorthStarGraphQL.Extention;
using NorthStarGraphQL.GraphQL;
using NorthStarGraphQL.Interface;
using NorthStarGraphQL.Repository;
using NorthStarGraphQL.Services;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<KafkaSetting>(builder.Configuration.GetSection("Kafka"));

builder.Services
    .AddGraphQLServer()
    .AddAuthorization()
    .AddQueryType<NorthStarQuery>()
    .AddMutationType<NorthStarMutation>()
    .AddSubscriptionType<NorthStarSubscription>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.Authority = builder.Configuration["IdentityManager:ServerUrl"];
        opt.Audience = "bankapi";

        opt.BackchannelHttpHandler = new HttpClientHandler { ServerCertificateCustomValidationCallback = delegate { return true; } };

        opt.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
    });

builder.Services.AddInMemorySubscriptions();

builder.Services.AddSingleton<ICustomerRepository, CustomerRepository>();

builder.Services.AddSingleton<IIdentityService, IdentityService>();
builder.Services.AddSingleton<IIdentityRepository, IdentityRepository>();

builder.Services.AddHostedService<KafkaMessagerService>();

builder.Services.AddClients(builder.Configuration);
// default setup
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "GraphQLNetExample", Version = "v1" });
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); 
app.UseAuthorization();

app.UseWebSockets();

app.UseEndpoints(endpoints => {
    endpoints.MapGraphQL();
    endpoints.MapBananaCakePop("/graphql/ui");
});

app.Run();