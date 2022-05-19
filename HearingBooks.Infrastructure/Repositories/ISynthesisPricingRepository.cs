using HearingBooks.Domain.Entities;
using HearingBooks.Domain.ValueObjects.TextSynthesis;

namespace HearingBooks.Infrastructure.Repositories;

public interface ISynthesisPricingRepository
{
	Task<SynthesisPricing> GetPricingForType(SynthesisType synthesisType);
}