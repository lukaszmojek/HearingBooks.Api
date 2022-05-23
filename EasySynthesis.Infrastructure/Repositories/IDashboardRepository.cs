namespace EasySynthesis.Infrastructure.Repositories;

public interface IDashboardRepository
{
	Task<SynthesesSummary> GetSynthesesSummary(Guid userId);
}