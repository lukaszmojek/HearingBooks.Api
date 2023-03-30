using HearingBooks.Api.Core.TimeProvider;

namespace HearingBooks.MailingService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly ITimeProvider _timeProvider;

    public Worker(ILogger<Worker> logger, ITimeProvider timeProvider)
    {
        _logger = logger;
        _timeProvider = timeProvider;
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