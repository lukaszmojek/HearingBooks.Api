using HearingBooks.Domain.DDD;
using HearingBooks.Domain.ValueObjects.Voice;

namespace HearingBooks.Domain.Entities;

public class Voice : Entity<Guid>
{
	public string Name { get; set; }
	public string DisplayName { get; set; }
	public VoiceType Type { get; set; }
	public bool IsMultilingual { get; set; }
}