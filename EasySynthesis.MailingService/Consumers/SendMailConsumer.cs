using EasySynthesis.Contracts.TextSynthesis;
using FluentEmail.Core;
using MassTransit;

namespace EasySynthesis.MailingService.Consumers;

public class SendMailConsumer :
	IConsumer<SendMailNotificationAboutTextSynthesis>
{
	readonly ILogger<SendMailConsumer> _logger;
	readonly IFluentEmail _fluentEmail;

	public SendMailConsumer(ILogger<SendMailConsumer> logger, IFluentEmail fluentEmail)
	{
		_logger = logger;
		_fluentEmail = fluentEmail;
	}

	public async Task Consume(ConsumeContext<SendMailNotificationAboutTextSynthesis> context)
	{
		var message = context.Message;
		
		_logger.LogInformation($"Consumed {nameof(SendMailNotificationAboutTextSynthesis)} message for user with id: {message.UserId}");

		var email = await _fluentEmail
			// .To(message.UserEmail)
			.To("lukasz.mojek@gmail.com")
			.Subject("Hej Darka Koparka!")
			.Body("Testujemy!")
			.SendAsync();

		var logMessage = "Email sent!";
		
		if (!email.Successful)
		{
			logMessage = "ERROR!";
		}
		
		_logger.LogInformation(logMessage);
	}
}