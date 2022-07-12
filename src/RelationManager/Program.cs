using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Sinks.Grafana.Loki;

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

var app = builder.Build();

app.UseSerilogRequestLogging();

app.MapGet("/", () => "Hello World!");

app.Run();
