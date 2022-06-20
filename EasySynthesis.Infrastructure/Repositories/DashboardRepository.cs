namespace EasySynthesis.Infrastructure.Repositories;

public class DashboardRepository : IDashboardRepository
{
	private readonly IDialogueSynthesisRepository _dialogueSynthesisRepository;
	private readonly ITextSynthesisRepository _textSynthesisRepository;
	public DashboardRepository(IDialogueSynthesisRepository dialogueSynthesisRepository, ITextSynthesisRepository textSynthesisRepository)
	{
		_dialogueSynthesisRepository = dialogueSynthesisRepository;
		_textSynthesisRepository = textSynthesisRepository;
	}
	
	public async Task<SynthesesSummary> GetSynthesesSummary(Guid userId)
	{
		var dialogueSyntheses = await _dialogueSynthesisRepository.GetAllForUser(userId);
		var textSyntheses = await _textSynthesisRepository.GetAllForUser(userId);

		var characterCount = 0;
		double textSynthesesPriceInUsd = 0;
		double dialogueSynthesesPriceInUsd = 0;
		var durationInSeconds = 0;
		
		if (textSyntheses.Any())
		{
			characterCount += textSyntheses
				.Select(x => x.CharacterCount)
				.Aggregate((current, next) => current + next);
			
			durationInSeconds += textSyntheses
				.Select(x => x.DurationInSeconds)
				.Aggregate((current, next) => current + next);
					
			textSynthesesPriceInUsd = textSyntheses
				.Select(x => x.PriceInUsd)
				.Aggregate((current, next) => current + next);
		}
		
		if (dialogueSyntheses.Any())
		{
			characterCount += dialogueSyntheses
				.Select(x => x.CharacterCount)
				.Aggregate((current, next) => current + next);
			
			durationInSeconds += dialogueSyntheses
				.Select(x => x.DurationInSeconds)
				.Aggregate((current, next) => current + next);
					
			dialogueSynthesesPriceInUsd = dialogueSyntheses
				.Select(x => x.PriceInUsd)
				.Aggregate((current, next) => current + next);
		}

		var synthesesSummary = new SynthesesSummary()
		{
			DialogueSynthesesCount = dialogueSyntheses.Count(),
			TextSynthesesCount = textSyntheses.Count(),
			DialogueSynthesesPriceInUsd = dialogueSynthesesPriceInUsd,
			TextSynthesesPriceInUsd = textSynthesesPriceInUsd,
			SynthesesCharactersCount = characterCount,
			SynthesesDurationInSeconds = durationInSeconds
		};

		return synthesesSummary;
	}
}