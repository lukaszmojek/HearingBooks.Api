using HearingBooks.Domain.ValueObjects.Syntheses;

namespace HearingBooks.Infrastructure;

public interface ISynthesisPricingService
{
	Task<decimal> GetPriceForSynthesis(SynthesisType synthesisType, int synthesisCharacterCount);
}