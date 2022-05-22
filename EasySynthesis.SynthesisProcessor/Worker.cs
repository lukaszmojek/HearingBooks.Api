using MassTransit;

namespace EasySynthesis.SynthesisProcessor;

public class Worker : BackgroundService
{
	private IBus _bus;

	public Worker(IBus bus)
	{
		_bus = bus;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while (!stoppingToken.IsCancellationRequested)
		{
			// _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
			await _bus.Publish<UserRequestedTextSynthesis>(new
			{
				UserId = Guid.NewGuid(),
				Email = "Dupa123"
			}, stoppingToken);
			
			await Task.Delay(1000, stoppingToken);
		}
	}
}