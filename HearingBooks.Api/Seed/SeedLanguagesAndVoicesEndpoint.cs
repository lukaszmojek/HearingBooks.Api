using HearingBooks.Domain.Entities;
using HearingBooks.Persistance;

namespace HearingBooks.Api.Seed;

public class SeedLanguagesAndVoicesEndpoint : EndpointWithoutRequest
{
	private readonly HearingBooksDbContext _context;

	public SeedLanguagesAndVoicesEndpoint(HearingBooksDbContext context)
	{
		_context = context;
	}

	public override void Configure()
	{
		Get("seed/languages-and-voices");
	}

	public override async Task HandleAsync(CancellationToken ct)
	{
		var enVoices = new[]
		{
			new Voice
			{
				Id = Guid.NewGuid(),
				Name = "pl-PL-AgnieszkaNeural",
				DisplayName = "Amber",
				Type = VoiceType.Female,
				IsMultilingual = false
			},
			new Voice
			{
				Id = Guid.NewGuid(),
				Name = "pl-PL-ZofiaNeural",
				DisplayName = "Zofia",
				Type = VoiceType.Female,
				IsMultilingual = false
			},
			new Voice
			{
				Id = Guid.NewGuid(),
				Name = "pl-PL-MarekNeural",
				DisplayName = "Marek",
				Type = VoiceType.Male,
				IsMultilingual = false
			}
		};

		var plVoices = new[]
		{
			new Voice
			{
				Id = Guid.NewGuid(),
				Name = "en-US-AmberNeural",
				DisplayName = "Amber",
				Type = VoiceType.Female,
				IsMultilingual = false
			},
			new Voice
			{
				Id = Guid.NewGuid(),
				Name = "en-US-AriaNeural",
				DisplayName = "Aria",
				Type = VoiceType.Female,
				IsMultilingual = false
			},
			new Voice
			{
				Id = Guid.NewGuid(),
				Name = "en-US-JennyMultilingualNeural",
				DisplayName = "Jenny",
				Type = VoiceType.Female,
				IsMultilingual = true
			},
			new Voice
			{
				Id = Guid.NewGuid(),
				Name = "en-US-AnaNeural",
				DisplayName = "Ana",
				Type = VoiceType.Kid,
				IsMultilingual = false
			},
			new Voice
			{
				Id = Guid.NewGuid(),
				Name = "en-US-BrandonNeural",
				DisplayName = "Brandon",
				Type = VoiceType.Male,
				IsMultilingual = false
			},
			new Voice
			{
				Id = Guid.NewGuid(),
				Name = "en-US-ChristopherNeural",
				DisplayName = "Christopher",
				Type = VoiceType.Male,
				IsMultilingual = false
			},
			new Voice
			{
				Id = Guid.NewGuid(),
				Name = "en-US-JacobNeural",
				DisplayName = "Jacob",
				Type = VoiceType.Male,
				IsMultilingual = false
			}
		};

		var languages = new List<Language>
		{
			new()
			{
				Id = Guid.NewGuid(),
				Name = "Polish",
				Symbol = "pl-PL",
				Voices = enVoices
			},
			new()
			{
				Id = Guid.NewGuid(),
				Name = "English",
				Symbol = "en-US",
				Voices = plVoices
			}
		};

		var languagesToDelete = _context.Languages
			.AsEnumerable()
			.Where(
				entity =>
					languages.Any(
						language =>
							language.Name == entity.Name &&
							language.Symbol == entity.Symbol
					)
			);

		var voicesToDelete = _context.Voices
			.AsEnumerable()
			.Where(
				entity =>
					enVoices.Concat(plVoices)
						.Any(
							voice =>
								voice.Name == entity.Name &&
								voice.Type == entity.Type
						)
			);

		_context.Voices.RemoveRange(voicesToDelete);
		_context.Languages.RemoveRange(languagesToDelete);
		await _context.SaveChangesAsync();

		await _context.Languages.AddRangeAsync(languages);
		await _context.SaveChangesAsync();

		await SendOkAsync();
	}
}