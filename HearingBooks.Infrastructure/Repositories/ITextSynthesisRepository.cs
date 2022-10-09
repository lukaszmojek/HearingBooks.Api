using HearingBooks.Domain.Entities;

namespace HearingBooks.Infrastructure.Repositories;

public interface ITextSynthesisRepository
{
	Task<TextSynthesis> GetById(Guid synthesisId);
	Task<IEnumerable<TextSynthesis>> GetAllForUser(Guid userId);
	Task Insert(TextSynthesis synthesis);
}