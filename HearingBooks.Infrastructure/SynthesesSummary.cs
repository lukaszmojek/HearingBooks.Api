namespace HearingBooks.Infrastructure;

public class SynthesesSummary
{
	public int DialogueSynthesesCount { get; set; }
	public int TextSynthesesCount { get; set; }
	public int SynthesesCharactersCount { get; set; }
	public long SynthesesDurationInSeconds { get; set; }
	public double TextSynthesesPriceInUsd { get; set; }
	public double DialogueSynthesesSynthesesPriceInUsd { get; set; }
}