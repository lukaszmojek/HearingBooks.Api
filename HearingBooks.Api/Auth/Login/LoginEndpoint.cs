using Api.Factories;
using Contracts.Responses;
using HearingBooks.Api.Syntheses;
using HearingBooks.Infrastructure.Repositories;

namespace HearingBooks.Api.Auth.Login;

public class LoginEndpoint : Endpoint<LoginUserRequest>
{
	private readonly IUserRepository _userRepository;
	private readonly IUserService _userService;

	public LoginEndpoint(IUserService userService, IUserRepository userRepository)
	{
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

			await SendAsync(ResponseFactory.CreateSuccessResponse(new LoginUserResponse {Token = token}));
		}
		catch (ArgumentException e)
		{
			await SendAsync(ResponseFactory.CreateFailureResponse<LoginUserResponse>(e.Message));
		}
	}
}