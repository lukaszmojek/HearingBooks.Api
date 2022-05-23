using AutoMapper;
using EasySynthesis.Infrastructure.Repositories;

namespace EasySynthesis.Api.Languages.GetLanguages;

public class GetLanguagesEndpoint : EndpointWithoutRequest
{
	private readonly ILanguageRepository _languageRepository;
	private readonly IMapper _mapper;

	public GetLanguagesEndpoint(ILanguageRepository languageRepository, IMapper mapper)
	{
		_languageRepository = languageRepository;
		_mapper = mapper;
	}

	public override void Configure()
	{
		Get("languages");
		Roles("HearingBooks", "Writer", "Subscriber", "PayAsYouGo");
	}

	public override async Task HandleAsync(CancellationToken cancellationToken)
	{
		var languages = await _languageRepository.GetLanguages();

		var languagesDto = _mapper.Map<IEnumerable<LangaugeDto>>(languages);

		await SendOkAsync(languagesDto, cancellationToken);
	}
}