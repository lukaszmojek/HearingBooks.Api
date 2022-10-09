using HearingBooks.Domain.Entities;
using HearingBooks.Domain.ValueObjects.Syntheses;

namespace HearingBooks.Infrastructure.Repositories;

public interface ISynthesisPricingRepository
{
	Task<SynthesisPricing> GetPricingForType(SynthesisType synthesisType);
}