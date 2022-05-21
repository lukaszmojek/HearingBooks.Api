using EasySynthesis.Domain.DDD;

namespace EasySynthesis.Domain.ValueObjects;

public abstract class BaseId<T> : ValueObject<T> where T : ValueObject<T>
{
	protected BaseId() { }
}