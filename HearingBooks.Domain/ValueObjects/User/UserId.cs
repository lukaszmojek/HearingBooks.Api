namespace HearingBooks.Domain.ValueObjects.User;

public class UserId : BaseId<UserId>
{
	public Guid Value { get; set; }

	public UserId(Guid value)
	{
		Value = value;
	}

	public UserId()
	{

	}
	
	protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
	{
		yield return Value;
	}
}