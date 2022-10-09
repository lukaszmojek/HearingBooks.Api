using AutoMapper;
using HearingBooks.Contracts;
using HearingBooks.Infrastructure.Repositories;

namespace HearingBooks.Api.Languages.GetLanguages;

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
		Roles("HearingBooks", "PayAsYouGo");
	}

	public override async Task HandleAsync(CancellationToken cancellationToken)
	{
		var languages = await _languageRepository.GetLanguages();

		var languagesDto = _mapper.Map<IEnumerable<LangaugeDto>>(languages);

		await SendOkAsync(languagesDto, cancellationToken);
	}
}