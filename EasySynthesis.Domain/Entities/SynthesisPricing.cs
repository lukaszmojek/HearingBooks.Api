using EasySynthesis.Domain.DDD;
using EasySynthesis.Domain.ValueObjects.Syntheses;

namespace EasySynthesis.Domain.Entities;

public class SynthesisPricing : Entity<Guid>
{
	public SynthesisType SynthesisType { get; set; }
	public int PriceInUsdPer1MCharacters { get; set; }
}