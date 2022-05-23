using AutoMapper;
using EasySynthesis.Contracts;
using EasySynthesis.Contracts.TextSynthesis;
using EasySynthesis.Domain.Entities;
using EasySynthesis.Infrastructure.Repositories;
using EasySynthesis.MassTransit;
using MassTransit;

namespace EasySynthesis.Api.Syntheses.TextSyntheses.GetTextSynthesesForUser;

public class GetTextSynthesesForUserEndpoint : EndpointWithoutRequest
{
	readonly ITextSynthesisRepository _textSynthesisRepository;
	readonly IMapper _mapper;
	
	public GetTextSynthesesForUserEndpoint(ITextSynthesisRepository textSynthesisRepository, IMapper mapper)
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
	
	public async Task HandleAsync2(CancellationToken ct)
	{
		var requestingUser = (User) HttpContext.Items["User"];

		var syntheses = await _textSynthesisRepository.GetAllForUser(requestingUser.Id);
		var synthesesDto = syntheses
			.Select(synthesis => new TextSynthesisDto
			{
				Id = synthesis.Id,
				RequestingUserId = synthesis.RequestingUserId,
				Status = synthesis.Status,
				Title = synthesis.Title,
				SynthesisText = synthesis.SynthesisText,
				BlobContainerName = synthesis.BlobContainerName,
				BlobName = synthesis.BlobName,
				Language = synthesis.Language,
				Voice = synthesis.Voice
			});
	
		await SendAsync(synthesesDto, 200, ct);
	}
}