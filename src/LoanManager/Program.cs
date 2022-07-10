using LoanManager.DataContexts;
using LoanManager.Extention;
using LoanManager.Interface;
using LoanManager.Repository;
using LoanManager.Services;
using LoanManager.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Northstar.Message;
using System;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDBDatabaseSetting"));
builder.Services.AddSingleton<IMongoDBContext, MongoDBContext>();

// Add services to the container.
builder.Services.AddSingleton<ILoanRequestService, LoanRequestService>();
builder.Services.AddSingleton<IDecisionService, DecisionService>();
builder.Services.AddSingleton<IGenerateId, GenerateMongoDBId>();

builder.Services.AddSingleton<ILoanRequestRepository, LoanRequestRepository>();
builder.Services.AddSingleton<ICustomerRepository, CustomerRepository>();
builder.Services.AddSingleton<IDecisionRepository, DecisionRepository>();
builder.Services.AddSingleton<IRejectRepository, RejectRepository>();

builder.Services.AddValidations();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.Authority = "https://localhost:5004";
        opt.Audience = "bankapi";

        opt.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
    });


builder.Services.AddClients(builder.Configuration);

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

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
