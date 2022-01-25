using HearingBooks.Domain.Entities;

namespace HearingBooks.Infrastructure.Repositories;

public interface ILanguageRepository
{
	Task<IEnumerable<Language>> GetLanguages();
}