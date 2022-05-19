using HearingBooks.Domain.ValueObjects.TextSynthesis;
using HearingBooks.Infrastructure.Repositories;

namespace HearingBooks.Infrastructure;

public class SynthesisPricingService
	: ISynthesisPricingService
{
	private ISynthesisPricingRepository _synthesisPricingRepository { get; }
	
	public SynthesisPricingService(ISynthesisPricingRepository synthesisPricingRepository)
	{
		_synthesisPricingRepository = synthesisPricingRepository;
	}
	
	public async Task<double> GetPriceForSynthesis(SynthesisType synthesisType, int synthesisCharacterCount)
	{
		var synthesisPricing = await _synthesisPricingRepository.GetPricingForType(synthesisType);

		var price = synthesisCharacterCount * synthesisPricing.PriceInUsdPer1MCharacters;

		var priceByMilion = price / 1_000_000;
		
		return priceByMilion < 0.01
			? 0.01
			: priceByMilion;
	}
}