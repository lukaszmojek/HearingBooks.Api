using EasySynthesis.Domain.Entities;

namespace EasySynthesis.Infrastructure.Repositories;

public interface ILanguageRepository
{
	Task<IEnumerable<Language>> GetLanguages();
}