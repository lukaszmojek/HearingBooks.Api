using EasySynthesis.Domain.Entities;
using EasySynthesis.Domain.ValueObjects.Syntheses;

namespace EasySynthesis.Infrastructure.Repositories;

public interface ISynthesisPricingRepository
{
	Task<SynthesisPricing> GetPricingForType(SynthesisType synthesisType);
}