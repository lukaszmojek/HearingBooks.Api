using EasySynthesis.Domain.ValueObjects.Syntheses;

namespace EasySynthesis.Infrastructure;

public interface ISynthesisPricingService
{
	Task<double> GetPriceForSynthesis(SynthesisType synthesisType, int synthesisCharacterCount);
}