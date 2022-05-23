using EasySynthesis.Domain.Entities;
using EasySynthesis.Persistance;
using Microsoft.EntityFrameworkCore;

namespace EasySynthesis.Infrastructure.Repositories;

public class LanguageRepository
	: ILanguageRepository
{
	private HearingBooksDbContext _context { get; set; }
	private DbSet<Language> _dbset { get; set; }
	
	public LanguageRepository(HearingBooksDbContext context)
	{
		_context = context;
		_dbset = _context.Set<Language>();
	}
	
	public async Task<IEnumerable<Language>> GetLanguages()
	{
		return await _dbset
			.Include(x => x.Voices)
			.ToListAsync();
	}

	public async Task<Language> GetBySymbol(string symbol)
	{
		return await _dbset
			.Include(x => x.Voices)
			.FirstAsync(x => x.Symbol == symbol);
	}
}