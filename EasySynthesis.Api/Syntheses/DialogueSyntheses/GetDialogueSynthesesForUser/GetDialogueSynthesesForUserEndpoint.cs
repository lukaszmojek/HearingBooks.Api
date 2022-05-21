using AutoMapper;
using EasySynthesis.Domain.Entities;
using EasySynthesis.Infrastructure.Repositories;

namespace EasySynthesis.Api.Syntheses.DialogueSyntheses.GetDialogueSynthesesForUser;

public class GetDialogueSynthesesForUserEndpoint : EndpointWithoutRequest
{
	private IDialogueSynthesisRepository _dialogueSynthesisRepository;
	private IMapper _mapper;

	public GetDialogueSynthesesForUserEndpoint(IDialogueSynthesisRepository dialogueSynthesisRepository, IMapper mapper)
	{
		_dialogueSynthesisRepository = dialogueSynthesisRepository;
		_mapper = mapper;
	}

	public override void Configure()
	{
		Get("dialogue-syntheses");
		Roles("HearingBooks", "Writer", "Subscriber", "PayAsYouGo");
	}

	public override async Task HandleAsync(CancellationToken ct)
	{
		var requestingUser = (User) HttpContext.Items["User"];
		               
		var syntheses = await _dialogueSynthesisRepository.GetAllForUser(requestingUser.Id);
		var synthesesDto = _mapper.Map<IEnumerable<DialogueSynthesisDto>>(syntheses);

		await SendAsync(synthesesDto, 200, ct);
	}
}