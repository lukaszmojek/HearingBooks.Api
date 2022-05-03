namespace HearingBooks.Infrastructure.Repositories;

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

		var synthesesSummary = new SynthesesSummary()
		{
			DialogueSynthesesCount = dialogueSyntheses.Count(),
			TextSynthesesCount = textSyntheses.Count(),
			SynthesizedCharactersCount = dialogueSyntheses
				.Select(x => x.CharacterCount)
				.Aggregate((current, next) => current + next),
			TimeOfSynthesesInSeconds = dialogueSyntheses
				.Select(x => x.DurationInSeconds)
				.Aggregate((current, next) => current + next)
		};

		return synthesesSummary;
	}
}