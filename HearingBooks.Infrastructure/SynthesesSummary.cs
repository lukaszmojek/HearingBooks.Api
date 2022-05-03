namespace HearingBooks.Infrastructure.Repositories;

public class SynthesesSummary
{
	public int DialogueSynthesesCount { get; set; }
	public int TextSynthesesCount { get; set; }
	public int SynthesizedCharactersCount { get; set; }
	public long TimeOfSynthesesInSeconds { get; set; }
}