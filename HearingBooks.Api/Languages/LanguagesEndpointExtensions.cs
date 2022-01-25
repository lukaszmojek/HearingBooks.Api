using AutoMapper;
using HearingBooks.Infrastructure.Repositories;

namespace HearingBooks.Api.Languages;

public static class LanguagesEndpointExtensions
{
	private static readonly string _baseEndpointGroupRoute = "languages";

	public static void MapLanguagesEndpoints(this WebApplication app)
	{
		app.MapGet(
			$"/{_baseEndpointGroupRoute}",
			async (ILanguageRepository languageRepository, IMapper mapper) =>
			{
				var languages = await languageRepository.GetLanguages();

				var languagesDto = mapper.Map<IEnumerable<LangaugeDto>>(languages);

				return Results.Ok(languagesDto);
			});
	}
}