using EasySynthesis.Contracts.DialogueSynthesis;
using EasySynthesis.LiveNotifications.Hubs;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace EasySynthesis.LiveNotifications.Consumers;

public class SendLiveNotificationAboutDialogueSynthesisConsumer :
	IConsumer<SendLiveNotificationAboutDialogueSynthesis>
{
	readonly ILogger<SendLiveNotificationAboutDialogueSynthesisConsumer> _logger;
	public IHubContext<SynthesesHub> SynthesesHubContext { get; }

	public SendLiveNotificationAboutDialogueSynthesisConsumer(ILogger<SendLiveNotificationAboutDialogueSynthesisConsumer> logger, IHubContext<SynthesesHub> synthesesHubContext)
	{
		_logger = logger;
		SynthesesHubContext = synthesesHubContext;
	}

	public async Task Consume(ConsumeContext<SendLiveNotificationAboutDialogueSynthesis> context)
	{
		var message = context.Message;
		
		_logger.LogInformation($"Consumed {nameof(SendLiveNotificationAboutDialogueSynthesis)}");
		await SynthesesHubContext.Clients.All.SendAsync("dialogue-synthesis-updated", message.UserId, message.DialogueSynthesis);
	}
}