using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using UserManager.Interfaces;

namespace UserManager.Backgrondservices;

public class KeyBackgroundService : IHostedService
{
    public ILogger<KeyBackgroundService> _logger;
    private readonly IKeyService _keyService;

    public KeyBackgroundService(ILogger<KeyBackgroundService> logger, IKeyService keyService)
    {
        _logger = logger;
        _keyService = keyService;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Start loading emails");
        _keyService.Load();

        StopAsync(cancellationToken);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stop loading emails");

        return Task.CompletedTask;
    }
}
