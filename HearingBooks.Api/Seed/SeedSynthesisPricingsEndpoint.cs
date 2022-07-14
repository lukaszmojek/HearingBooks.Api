using EasySynthesis.Domain.Entities;
using EasySynthesis.Domain.ValueObjects.Syntheses;
using EasySynthesis.Persistance;

namespace EasySynthesis.Api.Seed;

public class SeedSynthesisPricingsEndpoint : EndpointWithoutRequest
{
	private readonly HearingBooksDbContext _context;
	
	public SeedSynthesisPricingsEndpoint(HearingBooksDbContext context)
	{
		_context = context;
	}

	public override void Configure()
	{
		Get("seed/synthesis-pricings");
		Roles("EasySynthesis");
	}
	
	public override async Task HandleAsync(CancellationToken ct)
	{
		var synthesisPricings = new List<SynthesisPricing>
		{
		    new()
		    {
		        Id = Guid.NewGuid(),
				SynthesisType = SynthesisType.TextSynthesis,
				PriceInUsdPer1MCharacters = 30,
		    },
		    new()
		    {
			    Id = Guid.NewGuid(),
			    SynthesisType = SynthesisType.DialogueSynthesis,
			    PriceInUsdPer1MCharacters = 40,
		    },
		};
		
		var synthesisPricingsToDelete = _context.SynthesisPricings
		    .AsEnumerable()
		    .Where(entity => synthesisPricings.Any(synthesisPricing => synthesisPricing.SynthesisType == entity.SynthesisType));
		
		_context.SynthesisPricings.RemoveRange(synthesisPricingsToDelete);
		await _context.SaveChangesAsync();
		
		await _context.SynthesisPricings.AddRangeAsync(synthesisPricings);
		await _context.SaveChangesAsync();

		await SendOkAsync();
	}
}