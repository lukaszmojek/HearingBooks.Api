using AutoMapper;
using EasySynthesis.Api.Syntheses.DialogueSyntheses;
using EasySynthesis.Contracts.DialogueSynthesis;
using EasySynthesis.Contracts.TextSynthesis;
using EasySynthesis.Services;
using MassTransit;

namespace EasySynthesis.SynthesisProcessor.Consumers;

public class DialogueSynthesisRequestedConsumer :
    IConsumer<DialogueSynthesisRequested>
{
    readonly ILogger<DialogueSynthesisRequestedConsumer> _logger;
    readonly DialogueSynthesisService _dialogueSynthesisService;
    readonly IMapper _mapper;
    readonly IBus _bus;

    public DialogueSynthesisRequestedConsumer(ILogger<DialogueSynthesisRequestedConsumer> logger, DialogueSynthesisService dialogueSynthesisService, IMapper mapper, IBus bus)
    {
        _logger = logger;
        _dialogueSynthesisService = dialogueSynthesisService;
        _mapper = mapper;
        _bus = bus;
    }

    public async Task Consume(ConsumeContext<DialogueSynthesisRequested> context)
    {
        var message = context.Message;
		
        _logger.LogInformation($"Consumed {nameof(TextSynthesisRequested)} message for user with id: {message.UserId}");

        var dialogueSynthesis = await _dialogueSynthesisService.CreateRequest(message.DialogueSynthesisData, message.UserId, message.RequestId);
        var dialogueSynthesisDto = _mapper.Map<DialogueSynthesisDto>(dialogueSynthesis);

        var liveNotificationMessage = new SendLiveNotificationAboutDialogueSynthesis { UserId = message.UserId, DialogueSynthesis = dialogueSynthesisDto };
        await _bus.Publish(liveNotificationMessage);
        
        _logger.LogInformation($"TextSynthesis with id: {message.RequestId} and title: {message.DialogueSynthesisData.Title} was successfully created!");
    }
}