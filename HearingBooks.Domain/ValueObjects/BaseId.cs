using HearingBooks.Domain.DDD;

namespace HearingBooks.Domain.ValueObjects;

public abstract class BaseId<T> : ValueObject<T> where T : ValueObject<T>
{
	protected BaseId() { }
}