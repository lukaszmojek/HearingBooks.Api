using HearingBooks.Domain.Entities;
using HearingBooks.Persistance;
using Microsoft.EntityFrameworkCore;

namespace HearingBooks.Infrastructure.Repositories;

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
		return await _dbset.Include(x => x.Voices)
			.ToListAsync();
	}
}