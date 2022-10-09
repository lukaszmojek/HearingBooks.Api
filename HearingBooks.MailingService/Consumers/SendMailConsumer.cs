using FluentEmail.Core;
using HearingBooks.Contracts;
using MassTransit;

namespace HearingBooks.MailingService.Consumers;

public class SendMailConsumer :
	IConsumer<SendMailNotificationAboutSynthesis>
{
	readonly ILogger<SendMailConsumer> _logger;
	readonly IFluentEmail _fluentEmail;

	public SendMailConsumer(ILogger<SendMailConsumer> logger, IFluentEmail fluentEmail)
	{
		_logger = logger;
		_fluentEmail = fluentEmail;
	}

	public async Task Consume(ConsumeContext<SendMailNotificationAboutSynthesis> context)
	{
		var message = context.Message;

		_logger.LogInformation(
			$"Consumed {nameof(SendMailNotificationAboutSynthesis)} message for user with id: {message.UserId}");

		var template = 
			$"Hi {message.UserName},\n"
			+ $"We are informing you, that your Synthesis of title '{message.SynthesisTitle}' was successfully processed.\n"
			+ "Log in to the platform to access it.\n"
			+ "\n"
			+ "Cheers,\n"
			+ "HearingBooks";

		var email = await _fluentEmail
			.To(message.UserEmail)
			.Subject($"Your TextSynthesis with title '{message.SynthesisTitle}' was processed!")
			.Tag("SynthesisNotification")
			.Body(template)
			.SendAsync();

		var logMessage = "Email sent!";
		
		if (!email.Successful)
		{
			logMessage = email.ErrorMessages.Aggregate((x, y) => $"{x}\n{y}");
		}

		_logger.LogInformation(logMessage);
	}
}