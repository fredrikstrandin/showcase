using CustomerManager.DataContexts;
using CustomerManager.Interfaces;
using CustomerManager.Model;
using CustomerManager.Repository;
using CustomerManager.Services;
using CustomerManager.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("Kafka"));
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDBDatabaseSetting"));

builder.Services.AddSingleton<IMongoDBContext, MongoDBContext>();

// Add services to the container.
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICustomerRepository, CustomerMongoDBRepository>();

builder.Services.AddSingleton<IPasswordService, PasswordService>();

builder.Services.AddSingleton<IMessageService, MessageService>();
builder.Services.AddSingleton<IMessageRepository, MessageKafkaRepository>();

builder.Services.AddGrpc();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapGrpcService<CustomerService>();
app.MapControllers();

app.Run();
