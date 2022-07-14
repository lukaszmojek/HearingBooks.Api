using EasySynthesis.Domain.Entities;

namespace EasySynthesis.Infrastructure.Repositories;

public interface IDialogueSynthesisRepository
{
	Task<DialogueSynthesis> GetById(Guid synthesisId);
	Task<IEnumerable<DialogueSynthesis>> GetAllForUser(Guid userId);
	Task Insert(DialogueSynthesis synthesis);
}