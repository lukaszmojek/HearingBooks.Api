using AutoMapper;
using EasySynthesis.Contracts;
using EasySynthesis.Domain.Entities;

namespace EasySynthesis.Api.Users.GetUserDetails;

public class GetUserDetailsEndpoint : EndpointWithoutRequest
{
	private readonly IMapper _mapper;

	public GetUserDetailsEndpoint(IMapper mapper)
	{
		_mapper = mapper;
	}

	public override void Configure()
	{
		Get("user-details");
		Roles("EasySynthesis", "PayAsYouGo");
	}

	public override async Task HandleAsync(CancellationToken cancellationToken)
	{
		var user = (User) HttpContext.Items["User"];
		var userDto = _mapper.Map<UserDto>(user);
		
		await SendOkAsync(userDto, cancellationToken);
	}
}