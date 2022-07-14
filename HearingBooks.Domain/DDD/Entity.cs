using System.ComponentModel.DataAnnotations;

namespace EasySynthesis.Domain.DDD;

public abstract class Entity<T>
{
	[Key]
	public T Id { get; set; }
}