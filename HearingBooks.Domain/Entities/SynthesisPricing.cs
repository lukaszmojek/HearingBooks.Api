using HearingBooks.Domain.DDD;
using HearingBooks.Domain.ValueObjects.Syntheses;

namespace HearingBooks.Domain.Entities;

public class SynthesisPricing : Entity<Guid>
{
	public SynthesisType SynthesisType { get; set; }
	public int PriceInUsdPer1MCharacters { get; set; }
}