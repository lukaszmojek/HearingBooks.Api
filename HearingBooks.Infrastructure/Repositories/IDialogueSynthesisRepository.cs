using HearingBooks.Domain.Entities;

namespace HearingBooks.Infrastructure.Repositories;

public interface IDialogueSynthesisRepository
{
	Task<DialogueSynthesis> GetById(Guid synthesisId);
	Task<IEnumerable<DialogueSynthesis>> GetAllForUser(Guid userId);
	Task Insert(DialogueSynthesis synthesis);
}