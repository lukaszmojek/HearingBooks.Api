using System.ComponentModel.DataAnnotations;

namespace HearingBooks.Domain.DDD;

public abstract class Entity<T>
{
	[Key]
	public T Id { get; set; }
}