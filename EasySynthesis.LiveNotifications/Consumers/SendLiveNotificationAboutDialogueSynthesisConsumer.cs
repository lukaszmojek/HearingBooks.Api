using EasySynthesis.Contracts.TextSynthesis;
using MassTransit;

namespace EasySynthesis.LiveNotifications.Consumers;

public class SendLiveNotificationAboutDialogueSynthesisConsumer :
	IConsumer<SendLiveNotificationAboutDialogueSynthesis>
{
	readonly ILogger<SendLiveNotificationAboutDialogueSynthesisConsumer> _logger;

	public SendLiveNotificationAboutDialogueSynthesisConsumer(ILogger<SendLiveNotificationAboutDialogueSynthesisConsumer> logger)
	{
		_logger = logger;
	}

	public async Task Consume(ConsumeContext<SendLiveNotificationAboutDialogueSynthesis> context)
	{
		var message = context.Message;
		
		_logger.LogInformation($"Consumed {nameof(SendLiveNotificationAboutDialogueSynthesisConsumer)}");
		// await _SynthesesHubContext.Clients.All.SendAsync("messageReceived", message.UserId);
	}
}