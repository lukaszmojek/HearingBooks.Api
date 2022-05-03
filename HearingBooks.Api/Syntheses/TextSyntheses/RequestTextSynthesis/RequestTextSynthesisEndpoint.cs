using HearingBooks.Domain.Entities;

namespace HearingBooks.Api.Syntheses.TextSyntheses.RequestTextSynthesis;

public class RequestTextSynthesisEndpoint : Endpoint<TextSyntehsisRequest>
{
	private TextSynthesisService _textSynthesisService;

	public RequestTextSynthesisEndpoint(TextSynthesisService textSynthesisRepository)
	{
		_textSynthesisService = textSynthesisRepository;
	}

	public override void Configure()
	{
		Post("text-syntheses");
		Roles("HearingBooks", "Writer", "Subscriber", "PayAsYouGo");
	}

	public override async Task HandleAsync(TextSyntehsisRequest request, CancellationToken cancellationToken)
	{
		var requestingUser = (User) HttpContext.Items["User"];
		               
		var requestId = await _textSynthesisService.CreateRequest(request, requestingUser);
		var resourceRoute = $"text-syntheses/{requestId}";
		
		await SendCreatedAtAsync(resourceRoute, null, null);
	}
}