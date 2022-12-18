using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using HearingBooks.Api.Core.Auth;
using HearingBooks.Api.Core.Configuration;
using HearingBooks.Api.Core.Responses;
using HearingBooks.Infrastructure.Repositories;

namespace HearingBooks.Api.Core.Login;

public class LoginEndpoint : Endpoint<LoginUserRequest>
{
	private readonly IApiConfiguration _apiConfiguration;
	private readonly IUserRepository _userRepository;
	private readonly IUserService _userService;

	public LoginEndpoint(IApiConfiguration apiConfiguration, IUserService userService, IUserRepository userRepository)
	{
		_apiConfiguration = apiConfiguration;
		_userService = userService;
		_userRepository = userRepository;
	}

	public override void Configure()
	{
		Post("auth/login");
		AllowAnonymous();
	}

	public override async Task HandleAsync(LoginUserRequest request, CancellationToken cancellationToken)
	{
		try
		{
			if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
			{
				var errorMessage = "Email and Password have to be provided!";
				throw new ArgumentException(errorMessage);
			}

			var user = await _userRepository.GetUserByCredentialsAsync(request.Email, request.Password);

			if (user == null)
			{
				var errorMessage = "User with provided credentials do not exist!";
				throw new ArgumentException(errorMessage);
			}
			
			var token = _userService.Authenticate(user);

			await SendAsync(new
			{
				Username = request.Email,
				Token = token
			});
		}
		catch (ArgumentException e)
		{
			await SendAsync(ResponseFactory.CreateFailureResponse<LoginUserResponse>(e.Message));
		}
	}
}