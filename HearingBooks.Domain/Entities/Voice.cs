using HearingBooks.Domain.DDD;

namespace HearingBooks.Domain.Entities;

public class Voice : Entity<Guid>
{
	public string Name { get; set; }
	public string DisplayName { get; set; }
	public VoiceType Type { get; set; }
	public bool IsMultilingual { get; set; }
}