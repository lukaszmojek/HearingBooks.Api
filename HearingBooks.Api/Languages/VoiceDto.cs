using HearingBooks.Domain.Entities;

namespace HearingBooks.Api.Languages;

public record struct VoiceDto
{
	public string Name { get; set; }
	public string DisplayName { get; set; }
	public VoiceType Type { get; set; }
	public bool IsMultilingual { get; set; }
}