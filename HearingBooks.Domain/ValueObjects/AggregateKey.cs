namespace EasySynthesis.Domain.ValueObjects;

// public class AggregateKey : ValueObject<AggregateKey>
// {
// 	public string Id { get; set; }
// 	public string Type { get; set; }
//
//
// 	protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
// 	{
// 		yield return Id;
// 		yield return Type;
// 	}
//
// 	public static readonly AggregateKey Empty = new AggregateKey();
//
// 	public override string ToString()
// 	{
// 		return Id;
// 	}
// }