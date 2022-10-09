using HearingBooks.Domain.Entities;
using HearingBooks.Infrastructure.Repositories;

namespace HearingBooks.Api.Users.TopUp;

public class TopUpEndpoint : Endpoint<TopUpRequest>
{
	private readonly IUserRepository _userRepository;

	public TopUpEndpoint(IUserRepository userRepository)
	{
		_userRepository = userRepository;
	}

	public override void Configure()
	{
		Post("top-up");
		Roles("PayAsYouGo");
	}

	public override async Task HandleAsync(TopUpRequest topUpRequest, CancellationToken cancellationToken)
	{
		var user = (User) HttpContext.Items["User"];

		await _userRepository.TopUpAsync(user.Id, topUpRequest.Amount);
		
		await SendNoContentAsync(cancellationToken);
	}
}

public class TopUpRequest
{
	public int Amount { get; set; }
}