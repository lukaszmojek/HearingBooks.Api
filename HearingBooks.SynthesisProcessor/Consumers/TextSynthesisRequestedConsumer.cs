using AutoMapper;
using HearingBooks.Contracts;
using HearingBooks.Contracts.TextSynthesis;
using HearingBooks.Infrastructure.Repositories;
using HearingBooks.SynthesisProcessor.Services;
using MassTransit;

namespace HearingBooks.SynthesisProcessor.Consumers;

public class TextSynthesisRequestedConsumer :
	IConsumer<TextSynthesisRequested>
{
	readonly ILogger<TextSynthesisRequestedConsumer> _logger;
	readonly TextSynthesisService _textSynthesisService;
	readonly IUserRepository _userRepository;
	readonly IMapper _mapper;
	readonly IBus _bus;

	public TextSynthesisRequestedConsumer(ILogger<TextSynthesisRequestedConsumer> logger, TextSynthesisService textSynthesisService, IMapper mapper, IBus bus, IUserRepository userRepository)
	{
		_logger = logger;
		_textSynthesisService = textSynthesisService;
		_mapper = mapper;
		_bus = bus;
		_userRepository = userRepository;
	}

	public async Task Consume(ConsumeContext<TextSynthesisRequested> context)
	{
		var message = context.Message;
		
		_logger.LogInformation($"Consumed {nameof(TextSynthesisRequested)} message for user with id: {message.UserId}");
		
		var textSynthesis = await _textSynthesisService.CreateRequest(message.TextSynthesisData, message.UserId, message.RequestId);
		var textSynthesisDto = _mapper.Map<TextSynthesisDto>(textSynthesis);

		var liveNotificationMessage = new SendLiveNotificationAboutTextSynthesis { UserId = message.UserId, TextSynthesis = textSynthesisDto };
		await _bus.Publish(liveNotificationMessage);

		var requestingUser = await _userRepository.GetUserByIdAsync(message.UserId);
		if (requestingUser.Preference.EmailNotificationsEnabled)
		{
			await _bus.Publish(
				new SendMailNotificationAboutSynthesis
				{
					UserId = requestingUser.Id,
					UserEmail = requestingUser.Email,
					UserName = requestingUser.FirstName,
					RequestId = textSynthesis.Id,
					SynthesisTitle = textSynthesis.Title,
				}
			);
		}
		
		_logger.LogInformation($"TextSynthesis with id: {message.RequestId} and title: {message.TextSynthesisData.Title} was successfully processed!");
	}
}