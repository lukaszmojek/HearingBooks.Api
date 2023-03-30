using HearingBooks.Api.Core.TimeProvider;

namespace HearingBooks.LiveNotifications;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly ITimeProvider _timeProvider;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation($"Worker running at: {_timeProvider.UtcNow()}");
            await Task.Delay(100000, stoppingToken);
        }
    }
}