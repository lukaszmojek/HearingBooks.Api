namespace HearingBooks.Api.Languages;

public record struct LangaugeDto
{
	public string Name { get; set; }
	public string Symbol { get; set; }
	public IEnumerable<VoiceDto> Voices { get; set; }
}