using HearingBooks.Domain.Entities;
using HearingBooks.Domain.ValueObjects.Syntheses;
using HearingBooks.Persistance;
using Microsoft.EntityFrameworkCore;

namespace HearingBooks.Infrastructure.Repositories;

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