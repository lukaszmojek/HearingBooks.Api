using HearingBooks.Domain.Entities;

namespace HearingBooks.Api.Syntheses.DialogueSyntheses.RequestDialogueSynthesis;

public class RequestDialogueSynthesisEndpoint : Endpoint<DialogueSyntehsisRequest>
{
	private readonly DialogueSynthesisService _dialogueSynthesisService;

	public RequestDialogueSynthesisEndpoint(DialogueSynthesisService dialogueSynthesisService)
	{
		_dialogueSynthesisService = dialogueSynthesisService;
	}

	public override void Configure()
	{
		Post("dialogue-syntheses");
		Roles("HearingBooks", "Writer", "Subscriber", "PayAsYouGo");
	}

	public override async Task HandleAsync(DialogueSyntehsisRequest request, CancellationToken cancellationToken)
	{
		var requestingUser = (User) HttpContext.Items["User"];
		request.RequestingUserId = requestingUser.Id;
		               
		var requestId = await _dialogueSynthesisService.CreateRequest(request);
		var resourceRoute = $"text-syntheses/{requestId}";
		
		await SendCreatedAtAsync(resourceRoute, null, null);
	}
}