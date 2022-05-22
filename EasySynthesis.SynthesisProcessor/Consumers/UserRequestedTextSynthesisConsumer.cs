using MassTransit;

namespace EasySynthesis.SynthesisProcessor.Consumers;

public class UserRequestedTextSynthesisConsumer :
	IConsumer<UserRequestedTextSynthesis>
{
	readonly ILogger<UserRequestedTextSynthesisConsumer> _logger;

	public UserRequestedTextSynthesisConsumer(ILogger<UserRequestedTextSynthesisConsumer> logger)
	{
		_logger = logger;
	}

	public async Task Consume(ConsumeContext<UserRequestedTextSynthesis> context)
	{
		_logger.LogInformation($"Order Submitted: {context.Message.UserId} - {context.Message.Email}");

		// await context.Publish<OrderSubmitted>(new
		// {
		// 	context.Message.OrderId
		// });
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