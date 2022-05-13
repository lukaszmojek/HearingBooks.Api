using HearingBooks.Domain.DDD;

namespace HearingBooks.Domain.Entities;

public class Preference : Entity<Guid>
{
	public bool AcrylicEnabled { get; set; }
	public string Language { get; set; }
}