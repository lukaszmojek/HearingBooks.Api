using HearingBooks.Domain.ValueObjects.Syntheses;

namespace HearingBooks.Infrastructure;

public interface ISynthesisPricingService
{
	Task<double> GetPriceForSynthesis(SynthesisType synthesisType, int synthesisCharacterCount);
}