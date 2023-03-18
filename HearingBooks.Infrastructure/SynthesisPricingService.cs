using HearingBooks.Domain.ValueObjects.Syntheses;
using HearingBooks.Infrastructure.Repositories;

namespace HearingBooks.Infrastructure;

public class SynthesisPricingService
	: ISynthesisPricingService
{
	private readonly int _priceDivider = 1_000_000;
	private readonly double _minimalPrice = 0.01;
	private ISynthesisPricingRepository _synthesisPricingRepository { get; }
	
	public SynthesisPricingService(ISynthesisPricingRepository synthesisPricingRepository)
	{
		_synthesisPricingRepository = synthesisPricingRepository;
	}
	
	public async Task<decimal> GetPriceForSynthesis(SynthesisType synthesisType, int synthesisCharacterCount)
	{
		var synthesisPricing = await _synthesisPricingRepository.GetPricingForType(synthesisType);

		long price = synthesisCharacterCount * synthesisPricing.PriceInUsdPer1MCharacters;

		var priceByMilion = price / _priceDivider;
		
		return priceByMilion < _minimalPrice
			? (decimal) _minimalPrice
			: priceByMilion;
	}
}