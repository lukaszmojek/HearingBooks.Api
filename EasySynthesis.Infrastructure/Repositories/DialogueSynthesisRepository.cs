using EasySynthesis.Domain.Entities;
using EasySynthesis.Persistance;
using Microsoft.EntityFrameworkCore;

namespace EasySynthesis.Infrastructure.Repositories;

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
			.Include(x => x.User)
			.Include(x => x.Language)
			.Include(x => x.FirstSpeakerVoice)
			.Include(x => x.SecondSpeakerVoice)
			.FirstAsync(x => x.Id == synthesisId);
	}

	public async Task<IEnumerable<DialogueSynthesis>> GetAllForUser(Guid userId)
	{
		return await _set
			.Include(x => x.User)
			.Include(x => x.Language)
			.Include(x => x.FirstSpeakerVoice)
			.Include(x => x.SecondSpeakerVoice)
			.Where(x => x.User.Id == userId)
			.ToListAsync();
	}

	public async Task Insert(DialogueSynthesis synthesis)
	{
		await _set.AddAsync(synthesis);
	}
}