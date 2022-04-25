using AutoMapper;
using HearingBooks.Domain.Entities;
using HearingBooks.Infrastructure.Repositories;

namespace HearingBooks.Api.Syntheses.TextSyntheses.GetTextSynthesesForUser;

public class GetSynthesesForUserEndpoint : EndpointWithoutRequest
{
	private ITextSynthesisRepository _textSynthesisRepository;
	private IMapper _mapper;

	public GetSynthesesForUserEndpoint(ITextSynthesisRepository textSynthesisRepository, IMapper mapper)
	{
		_textSynthesisRepository = textSynthesisRepository;
		_mapper = mapper;
	}

	public override void Configure()
	{
		Get("text-syntheses");
		Roles("HearingBooks", "Writer", "Subscriber", "PayAsYouGo");
	}

	public override async Task HandleAsync(CancellationToken ct)
	{
		var requestingUser = (User) HttpContext.Items["User"];
		               
		var syntheses = await _textSynthesisRepository.GetAllForUser(requestingUser.Id);
		var synthesesDto = _mapper.Map<IEnumerable<TextSynthesisDto>>(syntheses);

		await SendAsync(synthesesDto, 200, ct);
	}
}