using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;
using IdentityManager;
using IdentityManager.Backgrondservices;
using IdentityManager.Extensions;
using IdentityManager.Interface;
using IdentityManager.Repository;
using IdentityManager.Services;
using IdentityServer4.MongoDB.MonogDBContext;
using IdentityServerHost.Quickstart.UI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<MongoDBDatabaseSetting>(builder.Configuration.GetSection("MongoDBDatabaseSetting"));

builder.Services.AddSingleton<MongoContext>();
builder.Services.AddSingleton<IUserStoreRepository, UserStoreMongoDBRepository>();

builder.Services.AddSingleton<IPasswordService, PasswordService>();

builder.Services.AddHostedService<KafkaMessagerService>();

builder.Services.AddControllers();

builder.Services.AddRazorPages();

builder.Services.AddIdentityServer(opt =>
{
    opt.Events.RaiseErrorEvents = true;
    opt.Events.RaiseInformationEvents = true;
    opt.Events.RaiseFailureEvents = true;
    opt.Events.RaiseSuccessEvents = true;

    opt.EmitStaticAudienceClaim = true;
})
    .AddMongoDBUsers()
    .AddInMemoryClients(Config.Clients)
    .AddInMemoryApiResources(Config.ApiResources)
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddInMemoryIdentityResources(Config.IdentityResources);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();


app.MapControllers();

app.UseIdentityServer();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages().RequireAuthorization();

app.Run();
