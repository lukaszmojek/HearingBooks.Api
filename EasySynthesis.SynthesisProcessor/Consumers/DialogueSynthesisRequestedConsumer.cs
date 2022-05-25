using EasySynthesis.Contracts.DialogueSynthesis;
using EasySynthesis.Contracts.TextSynthesis;
using EasySynthesis.Infrastructure.Repositories;
using EasySynthesis.Services;
using MassTransit;

namespace EasySynthesis.SynthesisProcessor.Consumers;

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