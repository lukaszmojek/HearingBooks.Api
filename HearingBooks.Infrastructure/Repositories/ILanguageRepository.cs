using HearingBooks.Domain.Entities;

namespace HearingBooks.Infrastructure.Repositories;

public interface ILanguageRepository
{
	Task<IEnumerable<Language>> GetLanguages();
	Task<Language> GetBySymbol(string symbol);
}