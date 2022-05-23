using EasySynthesis.Domain.Entities;
using EasySynthesis.Domain.ValueObjects.Syntheses;
using EasySynthesis.Persistance;
using Microsoft.EntityFrameworkCore;

namespace EasySynthesis.Infrastructure.Repositories;

public class SynthesisPricingRepository
	: ISynthesisPricingRepository
{
	private HearingBooksDbContext _context { get; set; }
	private DbSet<SynthesisPricing> _dbset { get; set; }
	
	public SynthesisPricingRepository(HearingBooksDbContext context)
	{
		_context = context;
		_dbset = _context.Set<SynthesisPricing>();
	}
	
	public async Task<SynthesisPricing> GetPricingForType(SynthesisType synthesisType)
	{
		return await _dbset.FirstOrDefaultAsync(x => x.SynthesisType == synthesisType);
	}
}