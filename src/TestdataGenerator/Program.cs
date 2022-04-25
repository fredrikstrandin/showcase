
using Microsoft.Extensions.Logging;
using TestdataGenerator;

using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder
        .AddFilter("Microsoft", LogLevel.Warning)
        .AddFilter("System", LogLevel.Warning)
        .AddFilter("NonHostConsoleApp.Program", LogLevel.Debug);
});

ILogger logger = loggerFactory.CreateLogger<Program>();


TestPerson testperson = new TestPerson(logger);

foreach (var pnr in testperson.Values)
{
    if (pnr.IsAdult)
    {
        
    }

    logger.LogInformation($"{pnr}");
}