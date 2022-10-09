namespace HearingBooks.Infrastructure.Repositories;

public interface IDashboardRepository
{
	Task<SynthesesSummary> GetSynthesesSummary(Guid userId);
}