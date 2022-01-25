using HearingBooks.Domain.Entities;
using HearingBooks.Persistance;
using Microsoft.EntityFrameworkCore;

namespace HearingBooks.Infrastructure.Repositories;

public class TextSynthesisRepository
	: ITextSynthesisRepository
{
	private readonly DbSet<TextSynthesis> _set;

	public TextSynthesisRepository(HearingBooksDbContext context)
	{
		_set = context.Set<TextSynthesis>();
	}

	public async Task<TextSynthesis> GetById(Guid synthesisId)
	{
		return await _set
			.FirstAsync(x => x.Id == synthesisId);
	}

	public async Task<IEnumerable<TextSynthesis>> GetAllForUser(Guid userId)
	{
		return await _set
			.Where(x => x.RequestingUserId == userId)
			.ToListAsync();
	}

	public async Task Insert(TextSynthesis synthesis)
	{
		await _set.AddAsync(synthesis);
	}
}