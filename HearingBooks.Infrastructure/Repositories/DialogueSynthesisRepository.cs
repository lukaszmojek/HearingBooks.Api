using HearingBooks.Domain.Entities;
using HearingBooks.Persistance;
using Microsoft.EntityFrameworkCore;

namespace HearingBooks.Infrastructure.Repositories;

public class DialogueSynthesisRepository : IDialogueSynthesisRepository
{
	private readonly DbSet<DialogueSynthesis> _set;

	public DialogueSynthesisRepository(HearingBooksDbContext context)
	{
		_set = context.Set<DialogueSynthesis>();
	}

	public async Task<DialogueSynthesis> GetById(Guid synthesisId)
	{
		return await _set
			.FirstAsync(x => x.Id == synthesisId);
	}

	public async Task<IEnumerable<DialogueSynthesis>> GetAllForUser(Guid userId)
	{
		return await _set
			.Where(x => x.RequestingUserId == userId)
			.ToListAsync();
	}

	public async Task Insert(DialogueSynthesis synthesis)
	{
		await _set.AddAsync(synthesis);
	}
}