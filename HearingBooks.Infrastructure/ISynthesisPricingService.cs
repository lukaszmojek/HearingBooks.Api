using HearingBooks.Domain.ValueObjects.TextSynthesis;

namespace HearingBooks.Infrastructure;

public interface ISynthesisPricingService
{
	Task<double> GetPriceForSynthesis(SynthesisType synthesisType, int synthesisCharacterCount);
}