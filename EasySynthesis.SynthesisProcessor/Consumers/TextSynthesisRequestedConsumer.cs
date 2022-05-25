using AutoMapper;
using EasySynthesis.Api.Syntheses.TextSyntheses;
using EasySynthesis.Contracts.TextSynthesis;
using EasySynthesis.Services;
using MassTransit;

namespace EasySynthesis.SynthesisProcessor.Consumers;

public class TextSynthesisRequestedConsumer :
	IConsumer<TextSynthesisRequested>
{
	readonly ILogger<TextSynthesisRequestedConsumer> _logger;
	readonly TextSynthesisService _textSynthesisService;
	readonly IMapper _mapper;
	readonly IBus _bus;

	public TextSynthesisRequestedConsumer(ILogger<TextSynthesisRequestedConsumer> logger, TextSynthesisService textSynthesisService, IMapper mapper, IBus bus)
	{
		_logger = logger;
		_textSynthesisService = textSynthesisService;
		_mapper = mapper;
		_bus = bus;
	}

	public async Task Consume(ConsumeContext<TextSynthesisRequested> context)
	{
		var message = context.Message;
		
		_logger.LogInformation($"Consumed {nameof(TextSynthesisRequested)} message for user with id: {message.UserId}");
		
		var textSynthesis = await _textSynthesisService.CreateRequest(message.TextSynthesisData, message.UserId, message.RequestId);
		var textSynthesisDto = _mapper.Map<TextSynthesisDto>(textSynthesis);

		var liveNotificationMessage = new SendLiveNotificationAboutTextSynthesis { UserId = message.UserId, TextSynthesis = textSynthesisDto };
		await _bus.Publish(liveNotificationMessage);
		
		_logger.LogInformation($"TextSynthesis with id: {message.RequestId} and title: {message.TextSynthesisData.Title} was successfully created!");
	}
}