using CommonLib.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UserManager.Backgrondservices;
using UserManager.DataContexts;
using UserManager.GrpcService;
using UserManager.Interfaces;
using UserManager.Repository;
using UserManager.Services;
using UserManager.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<KafkaSetting>(builder.Configuration.GetSection("Kafka"));
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDBDatabaseSetting"));

builder.Services.AddSingleton<IMongoDBContext, MongoDBContext>();

// Add services to the container.
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<IUserRepository, UserMongoDBRepository>();
builder.Services.AddSingleton<IKeyService, KeyService>();
builder.Services.AddSingleton<IKeyRepository, KeyMongoDBRepository>();


builder.Services.AddSingleton<IPasswordService, PasswordService>();
builder.Services.AddSingleton<IMessageService, MessageService>();

builder.Services.AddSingleton<IMessageRepository, MessageKafkaRepository>();

builder.Services.AddHostedService<KeyBackgroundService>();

builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseRouting();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<UserGrpcServer>();
    endpoints.MapGrpcReflectionService();

    endpoints.MapControllers();
    endpoints.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
    });
});

app.Run();
