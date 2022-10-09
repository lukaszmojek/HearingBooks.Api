using HearingBooks.Contracts.TextSynthesis;
using HearingBooks.LiveNotifications.Hubs;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace HearingBooks.LiveNotifications.Consumers;

public class SendLiveNotificationAboutTextSynthesisConsumer :
    IConsumer<SendLiveNotificationAboutTextSynthesis>
{
    readonly ILogger<SendLiveNotificationAboutTextSynthesisConsumer> _logger;
    public IHubContext<SynthesesHub> TextSynthesesHubContext { get; }
	
    public SendLiveNotificationAboutTextSynthesisConsumer(ILogger<SendLiveNotificationAboutTextSynthesisConsumer> logger, IHubContext<SynthesesHub> textSynthesesHubContext)
    {
        _logger = logger;
        TextSynthesesHubContext = textSynthesesHubContext;
    }

    public async Task Consume(ConsumeContext<SendLiveNotificationAboutTextSynthesis> context)
    {
        var message = context.Message;
        
        _logger.LogInformation($"Consumed {nameof(SendLiveNotificationAboutTextSynthesis)}");
        await TextSynthesesHubContext.Clients.All.SendAsync("text-synthesis-updated", message.UserId, message.TextSynthesis);
    }
}