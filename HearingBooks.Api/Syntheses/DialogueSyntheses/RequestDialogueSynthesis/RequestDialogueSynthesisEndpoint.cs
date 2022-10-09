using AutoMapper;
using HearingBooks.Contracts;
using HearingBooks.Contracts.DialogueSynthesis;
using HearingBooks.Domain.Entities;
using MassTransit;

namespace HearingBooks.Api.Syntheses.DialogueSyntheses.RequestDialogueSynthesis;

public class RequestDialogueSynthesisEndpoint : Endpoint<DialogueSyntehsisRequest>
{
	private readonly IBus _bus;
	private readonly IMapper _mapper;

	public RequestDialogueSynthesisEndpoint(IBus bus, IMapper mapper)
	{
		_bus = bus;
		_mapper = mapper;
	}

	public override void Configure()
	{
		Post("dialogue-syntheses");
		Roles("HearingBooks", "Writer", "Subscriber", "PayAsYouGo");
	}

	public override async Task HandleAsync(DialogueSyntehsisRequest request, CancellationToken cancellationToken)
	{
		var requestingUser = (User) HttpContext.Items["User"];
		               
		var dialogueSynthesisData = _mapper.Map<DialogueSynthesisData>(request);
		var requestId = Guid.NewGuid();

		await _bus.Publish(
			new DialogueSynthesisRequested()
			{
				UserId = requestingUser.Id,
				RequestId = requestId,
				DialogueSynthesisData = dialogueSynthesisData
			}
		);
		var resourceRoute = $"dialogue-syntheses/{requestId}";
		
		await SendCreatedAtAsync(resourceRoute, null, null);
	}
}