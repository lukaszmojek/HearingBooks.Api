using EasySynthesis.Contracts.TextSynthesis;
using EasySynthesis.LiveNotifications.Hubs;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace EasySynthesis.LiveNotifications.Consumers;

public class SendLiveNotificationAboutTextSynthesisConsumer :
    IConsumer<SendLiveNotificationAboutTextSynthesis>
{
    readonly ILogger<SendLiveNotificationAboutTextSynthesisConsumer> _logger;
    public IHubContext<TextSynthesesHub> SynthesesHubContext { get; }
	
    public SendLiveNotificationAboutTextSynthesisConsumer(ILogger<SendLiveNotificationAboutTextSynthesisConsumer> logger, IHubContext<TextSynthesesHub> synthesesHubContext)
    {
        _logger = logger;
        SynthesesHubContext = synthesesHubContext;
    }

    public async Task Consume(ConsumeContext<SendLiveNotificationAboutTextSynthesis> context)
    {
        var message = context.Message;
        _logger.LogInformation($"Consumed {nameof(SendLiveNotificationAboutTextSynthesisConsumer)}");
        await SynthesesHubContext.Clients.All.SendAsync("messageReceived", "dupa", "fior");
    }
}