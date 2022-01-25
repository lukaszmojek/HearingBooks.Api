using HearingBooks.Domain.DDD;

namespace HearingBooks.Domain.Entities;

public class Language : Entity<Guid>
{
	public string Name { get; set; }
	public string Symbol { get; set; }
	public IEnumerable<Voice> Voices { get; set; }
}