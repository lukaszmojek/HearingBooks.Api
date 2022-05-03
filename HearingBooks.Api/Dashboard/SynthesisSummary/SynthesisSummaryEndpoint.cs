using HearingBooks.Domain.Entities;
using HearingBooks.Infrastructure.Repositories;

namespace HearingBooks.Api.Syntheses.TextSyntheses.DownloadTextSynthesisFile;

public class SynthesisSummaryEndpoint : Endpoint<SynthesesSummaryRequest>
{
	private IDashboardRepository _dashboardRepository;

	public SynthesisSummaryEndpoint(IDashboardRepository dashboardRepository)
	{
		_dashboardRepository = dashboardRepository;
	}

	public override void Configure()
	{
		Get("syntheses-summary/{UserId}");
		Roles("HearingBooks", "Writer", "Subscriber", "PayAsYouGo");
	}

	public override async Task HandleAsync(SynthesesSummaryRequest request, CancellationToken cancellationToken)
	{
		var requestingUser = (User) HttpContext.Items["User"];

		if (requestingUser.Id != request.UserId)
		{
			await SendForbiddenAsync();
			return;
		}

		var synthesisSummary = await _dashboardRepository.GetSynthesesSummary(request.UserId);

		await SendAsync(synthesisSummary, cancellation: cancellationToken);
	}
}

public class SynthesesSummaryRequest
{
	public Guid UserId { get; set; }
}