using AutoMapper;
using HearingBooks.Contracts;
using HearingBooks.Contracts.DialogueSynthesis;
using HearingBooks.Contracts.TextSynthesis;
using HearingBooks.Infrastructure.Repositories;
using HearingBooks.SynthesisProcessor.Services;
using MassTransit;

namespace HearingBooks.SynthesisProcessor.Consumers;

public class DialogueSynthesisRequestedConsumer :
    IConsumer<DialogueSynthesisRequested>
{
    readonly ILogger<DialogueSynthesisRequestedConsumer> _logger;
    readonly DialogueSynthesisService _dialogueSynthesisService;
    readonly IUserRepository _userRepository;
    readonly IMapper _mapper;
    readonly IBus _bus;

    public DialogueSynthesisRequestedConsumer(ILogger<DialogueSynthesisRequestedConsumer> logger, DialogueSynthesisService dialogueSynthesisService, IMapper mapper, IBus bus, IUserRepository userRepository)
    {
        _logger = logger;
        _dialogueSynthesisService = dialogueSynthesisService;
        _mapper = mapper;
        _bus = bus;
        _userRepository = userRepository;
    }

    public async Task Consume(ConsumeContext<DialogueSynthesisRequested> context)
    {
        var message = context.Message;
		
        _logger.LogInformation($"Consumed {nameof(TextSynthesisRequested)} message for user with id: {message.UserId}");

        var dialogueSynthesis = await _dialogueSynthesisService.CreateRequest(message.DialogueSynthesisData, message.UserId, message.RequestId);
        var dialogueSynthesisDto = _mapper.Map<DialogueSynthesisDto>(dialogueSynthesis);

        var liveNotificationMessage = new SendLiveNotificationAboutDialogueSynthesis { UserId = message.UserId, DialogueSynthesis = dialogueSynthesisDto };
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
                    RequestId = dialogueSynthesis.Id,
                    SynthesisTitle = dialogueSynthesis.Title,
                }
            );
        }
        
        _logger.LogInformation($"DialogueSynthesis with id: {message.RequestId} and title: {message.DialogueSynthesisData.Title} was successfully processed!");
    }
}