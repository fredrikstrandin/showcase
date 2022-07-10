using IdentityManager;
using IdentityManager.Extensions;
using IdentityManager.GrpcServices;
using IdentityManager.Interface;
using IdentityManager.Repository;
using IdentityManager.Services;
using IdentityServer4.MongoDB.MonogDBContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<MongoDBDatabaseSetting>(builder.Configuration.GetSection("MongoDBDatabaseSetting"));

builder.Services.AddSingleton<MongoContext>();
builder.Services.AddSingleton<IUserStoreRepository, UserStoreMongoDBRepository>();

builder.Services.AddSingleton<IPasswordService, PasswordService>();

builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

builder.Services.AddControllers();

builder.Services.AddRazorPages();

builder.Services.AddIdentityServer(options =>
{
    options.Events.RaiseErrorEvents = true;
    options.Events.RaiseInformationEvents = true;
    options.Events.RaiseFailureEvents = true;
    options.Events.RaiseSuccessEvents = true;

    options.EmitStaticAudienceClaim = true;
    // set path where to store keys
    options.KeyManagement.KeyPath = $".\\keys";

    // new key every 30 days
    options.KeyManagement.RotationInterval = TimeSpan.FromDays(30);

    // announce new key 2 days in advance in discovery
    options.KeyManagement.PropagationTime = TimeSpan.FromDays(2);

    // keep old key for 7 days in discovery for validation of tokens
    options.KeyManagement.RetentionDuration = TimeSpan.FromDays(7);
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

app.UseIdentityServer();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages().RequireAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapGrpcService<IdentityGrpcServer>();
    endpoints.MapGrpcReflectionService();     
});
app.Run();
