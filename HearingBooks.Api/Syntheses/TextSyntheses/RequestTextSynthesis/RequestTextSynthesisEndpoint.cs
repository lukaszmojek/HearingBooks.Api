using AutoMapper;
using HearingBooks.Contracts;
using HearingBooks.Contracts.TextSynthesis;
using HearingBooks.Domain.Entities;
using MassTransit;

namespace HearingBooks.Api.Syntheses.TextSyntheses.RequestTextSynthesis;

public class RequestTextSynthesisEndpoint : Endpoint<TextSynthesisRequest>
{
	private readonly IBus _bus;
	private readonly IMapper _mapper;

	public RequestTextSynthesisEndpoint(IBus bus, IMapper mapper)
	{
		_bus = bus;
		_mapper = mapper;
	}

	public override void Configure()
	{
		Post("text-syntheses");
		Roles("HearingBooks", "Writer", "Subscriber", "PayAsYouGo");
	}

	public override async Task HandleAsync(TextSynthesisRequest request, CancellationToken cancellationToken)
	{
		var requestingUser = (User) HttpContext.Items["User"];
		               
		var textSynthesisData = _mapper.Map<TextSynthesisData>(request);
		var requestId = Guid.NewGuid();

		await _bus.Publish(
			new TextSynthesisRequested
			{
				UserId = requestingUser.Id,
				RequestId = requestId,
				TextSynthesisData = textSynthesisData
			}
		);
		
		var resourceRoute = $"text-syntheses/{requestId}";
		
		await SendCreatedAtAsync(resourceRoute, null, null);
	}
}