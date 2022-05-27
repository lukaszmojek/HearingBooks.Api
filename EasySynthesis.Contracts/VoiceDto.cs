using EasySynthesis.Domain.ValueObjects.Voice;

namespace EasySynthesis.Contracts;

public record struct VoiceDto
{
	public string Name { get; set; }
	public string DisplayName { get; set; }
	public VoiceType Type { get; set; }
	public bool IsMultilingual { get; set; }
}