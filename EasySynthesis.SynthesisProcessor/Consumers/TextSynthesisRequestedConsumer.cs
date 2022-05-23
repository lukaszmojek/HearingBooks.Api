using EasySynthesis.Contracts;
using EasySynthesis.Contracts.DialogueSynthesis;
using EasySynthesis.Contracts.TextSynthesis;
using EasySynthesis.Infrastructure.Repositories;
using EasySynthesis.Services;
using MassTransit;

namespace EasySynthesis.SynthesisProcessor.Consumers;

public class TextSynthesisRequestedConsumer :
	IConsumer<TextSynthesisRequested>
{
	readonly ILogger<TextSynthesisRequestedConsumer> _logger;
	readonly TextSynthesisService _textSynthesisService;
	readonly IUserRepository _userRepository;

	public TextSynthesisRequestedConsumer(ILogger<TextSynthesisRequestedConsumer> logger, TextSynthesisService textSynthesisService, IUserRepository userRepository)
	{
		_logger = logger;
		_textSynthesisService = textSynthesisService;
		_userRepository = userRepository;
	}

	public async Task Consume(ConsumeContext<TextSynthesisRequested> context)
	{
		var message = context.Message;
		
		_logger.LogInformation($"Consumed {nameof(TextSynthesisRequested)} message for user with id: {message.UserId}");

		var user = await _userRepository.GetUserByIdAsync(message.UserId);
		_ = await _textSynthesisService.CreateRequest(message.TextSynthesisData, user, message.RequestId);

		_logger.LogInformation($"TextSynthesis with id: {message.RequestId} and title: {message.TextSynthesisData.Title} was successfully created!");
	}
}

public class DialogueSynthesisRequestedConsumer :
	IConsumer<DialogueSynthesisRequested>
{
	readonly ILogger<DialogueSynthesisRequestedConsumer> _logger;
	readonly DialogueSynthesisService _dialogueSynthesisService;
	readonly IUserRepository _userRepository;

	public DialogueSynthesisRequestedConsumer(ILogger<DialogueSynthesisRequestedConsumer> logger, DialogueSynthesisService dialogueSynthesisService, IUserRepository userRepository)
	{
		_logger = logger;
		_dialogueSynthesisService = dialogueSynthesisService;
		_userRepository = userRepository;
	}

	public async Task Consume(ConsumeContext<DialogueSynthesisRequested> context)
	{
		var message = context.Message;
		
		_logger.LogInformation($"Consumed {nameof(TextSynthesisRequested)} message for user with id: {message.UserId}");

		var user = await _userRepository.GetUserByIdAsync(message.UserId);
		_ = await _dialogueSynthesisService.CreateRequest(message.DialogueSynthesisData, user, message.RequestId);

		_logger.LogInformation($"TextSynthesis with id: {message.RequestId} and title: {message.DialogueSynthesisData.Title} was successfully created!");
	}
}

// public class UserRequestedTextSynthesisConsumerDefinition :
// 	ConsumerDefinition<UserRequestedTextSynthesisConsumer>
// {
// 	public UserRequestedTextSynthesisConsumerDefinition()
// 	{
// 		// override the default endpoint name
// 		EndpointName = "order-service";
//
// 		// limit the number of messages consumed concurrently
// 		// this applies to the consumer only, not the endpoint
// 		ConcurrentMessageLimit = 8;
// 	}
//
// 	protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
// 		IConsumerConfigurator<UserRequestedTextSynthesisConsumer> consumerConfigurator)
// 	{
// 		// configure message retry with millisecond intervals
// 		endpointConfigurator.UseMessageRetry(r => r.Intervals(100,200,500,800,1000));
//
// 		// use the outbox to prevent duplicate events from being published
// 		endpointConfigurator.UseInMemoryOutbox();
// 	}
// }