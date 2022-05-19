using System;
using System.Threading.Tasks;
using HearingBooks.Domain.Entities;
using HearingBooks.Domain.ValueObjects.TextSynthesis;
using HearingBooks.Infrastructure.Repositories;
using Moq;
using Xunit;

namespace HearingBooks.Infrastructure.Tests;

public class SynthesisPricingServiceTests
{
	private Mock<ISynthesisPricingRepository> _synthesisPricingRepositoryMock;
	private ISynthesisPricingService _service;

	public SynthesisPricingServiceTests()
	{
		_synthesisPricingRepositoryMock = new Mock<ISynthesisPricingRepository>();
		_synthesisPricingRepositoryMock.Setup(x => x.GetPricingForType(SynthesisType.TextSynthesis))
			.ReturnsAsync(() => new SynthesisPricing {Id = Guid.NewGuid(), SynthesisType = SynthesisType.TextSynthesis, PriceInUsdPer1MCharacters = 30 });
		_synthesisPricingRepositoryMock.Setup(x => x.GetPricingForType(SynthesisType.DialogueSynthesis))
			.ReturnsAsync(() => new SynthesisPricing {Id = Guid.NewGuid(), SynthesisType = SynthesisType.DialogueSynthesis, PriceInUsdPer1MCharacters = 40 });

		_service = new SynthesisPricingService(_synthesisPricingRepositoryMock.Object);
	}

	[Fact]
	public async Task GetPriceForSynthesis_Should_Return_1_Cent_When_SynthesisType_Is_TextSynthesis_And_Text_Is_Short()
	{
		var price = await _service.GetPriceForSynthesis(SynthesisType.TextSynthesis, 10);
		
		Assert.Equal(0.01, price);
	}
	
	[Fact]
	public async Task GetPriceForSynthesis_Should_Return_1_Cent_When_SynthesisType_Is_DialogueSynthesis_And_Text_Is_Short()
	{
		var price = await _service.GetPriceForSynthesis(SynthesisType.DialogueSynthesis, 10);
		
		Assert.Equal(0.01, price);
	}
	
	[Fact]
	public async Task GetPriceForSynthesis_Should_Return_30_USD_When_SynthesisType_Is_TextSynthesis_And_Text_Is_1M_Characters_Long()
	{
		var price = await _service.GetPriceForSynthesis(SynthesisType.TextSynthesis, 1_000_000);
		
		Assert.Equal(30, price);
	}
	
	[Fact]
	public async Task GetPriceForSynthesis_Should_Return_40_USD_When_SynthesisType_Is_DialogueSynthesis_And_Text_Is_1M_Characters_Long()
	{
		var price = await _service.GetPriceForSynthesis(SynthesisType.DialogueSynthesis, 1_000_000);
		
		Assert.Equal(40, price);
	}
	
	[Fact]
	public async Task GetPriceForSynthesis_Should_Return_15_USD_When_SynthesisType_Is_TextSynthesis_And_Text_Is_Half_Milion_Characters_Long()
	{
		var price = await _service.GetPriceForSynthesis(SynthesisType.TextSynthesis, 500_000);
		
		Assert.Equal(15, price);
	}
	
	[Fact]
	public async Task GetPriceForSynthesis_Should_Return_20_USD_When_SynthesisType_Is_DialogueSynthesis_And_Text_Is_Half_Milion_Characters_Long()
	{
		var price = await _service.GetPriceForSynthesis(SynthesisType.DialogueSynthesis, 500_000);
		
		Assert.Equal(20, price);
	}
}