namespace EasySynthesis.Api.Languages.GetLanguages;

public record struct LangaugeDto
{
	public string Name { get; set; }
	public string Symbol { get; set; }
	public IEnumerable<VoiceDto> Voices { get; set; }
}