using Prometheus;
using RelationManager.Backgrondservices;
using RelationManager.Interface;
using RelationManager.Services;
using RelationManager.Repository;
using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Sinks.Grafana.Loki;
using System.Text.Json;
using CommonLib.Settings;
using System.Configuration;
using Neo4j.Driver;
using RelationManager.Settings;
using RelationManager.DataAccess;
using UserManager.GrpcService;

var builder = WebApplication.CreateBuilder(args);

//Creating the Logger with Minimum Settings
Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
                    .Enrich.FromLogContext()
                    .WriteTo.Console()
                    .CreateLogger();

builder.Host.UseSerilog((ctx, cfg) =>
{
    //Override Few of the Configurations
    cfg.Enrich.WithProperty("Application", ctx.HostingEnvironment.ApplicationName)
       .Enrich.WithProperty("Environment", ctx.HostingEnvironment.EnvironmentName)
       .WriteTo.Console(new RenderedCompactJsonFormatter())
       .WriteTo.GrafanaLoki(ctx.Configuration["loki"]);

});
// Register application setting using IOption provider mechanism
builder.Services.Configure<Neo4jSetting>(builder.Configuration.GetSection("Neo4jSettings"));

// Fetch settings object from configuration
var settings = new Neo4jSetting();
builder.Configuration.GetSection("Neo4jSettings").Bind(settings);

// This is to register your Neo4j Driver Object as a singleton
builder.Services.AddSingleton(GraphDatabase.Driver(settings.Connection, AuthTokens.Basic(settings.User, settings.Password)));

// This is your Data Access Wrapper over Neo4j session, that is a helper class for executing parameterized Neo4j Cypher queries in Transactions
builder.Services.AddScoped<INeo4jDataAccess, Neo4jDataAccess>();

builder.Services.AddHealthChecks()
        .ForwardToPrometheus();

builder.Services.Configure<KafkaSetting>(builder.Configuration.GetSection("Kafka"));

builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<IUserRepository, UserNeo4jRepository>();
builder.Services.AddSingleton<IFollowingService, FollowingService>();
builder.Services.AddSingleton<IFollowingRepository, FollowingNeo4jRepository>();

builder.Services.AddHostedService<KafkaMessagerService>();

builder.Services.AddGrpc();

var app = builder.Build();


app.UseSerilogRequestLogging();

app.UseRouting();
app.UseHttpMetrics();

app.UseEndpoints(endpoints =>
{
    endpoints.MapMetrics();
    endpoints.MapGrpcService<FollowingGrpcServer>();
    
    endpoints.MapGet("/", () => "Hello World!");
});


app.Run();
