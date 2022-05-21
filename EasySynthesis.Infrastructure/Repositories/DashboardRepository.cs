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

		var dialogueSynthesesCharacterCount = dialogueSyntheses
			.Select(x => x.CharacterCount)
			.Aggregate((current, next) => current + next);
		
		var textSynthesesCharacterCount = textSyntheses
			.Select(x => x.CharacterCount)
			.Aggregate((current, next) => current + next);
		
		var dialogueSynthesesDurationInSeconds = dialogueSyntheses
			.Select(x => x.DurationInSeconds)
			.Aggregate((current, next) => current + next);
		
		var textSynthesesDurationInSeconds = textSyntheses
			.Select(x => x.DurationInSeconds)
			.Aggregate((current, next) => current + next);
		
		var dialgueSynthesesPriceInUsd = dialogueSyntheses
			.Select(x => x.PriceInUsd)
			.Aggregate((current, next) => current + next);
		
		var textSynthesesPriceInUsd = textSyntheses
			.Select(x => x.PriceInUsd)
			.Aggregate((current, next) => current + next);
		
		var synthesesSummary = new SynthesesSummary()
		{
			DialogueSynthesesCount = dialogueSyntheses.Count(),
			TextSynthesesCount = textSyntheses.Count(),
			DialogueSynthesesPriceInUsd = dialgueSynthesesPriceInUsd,
			TextSynthesesPriceInUsd = textSynthesesPriceInUsd,
			SynthesesCharactersCount = dialogueSynthesesCharacterCount + textSynthesesCharacterCount,
			SynthesesDurationInSeconds = dialogueSynthesesDurationInSeconds + textSynthesesDurationInSeconds
		};

		return synthesesSummary;
	}
}