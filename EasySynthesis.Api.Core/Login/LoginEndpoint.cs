using System;
using System.Threading;
using System.Threading.Tasks;
using EasySynthesis.Api.Core.Auth;
using EasySynthesis.Api.Core.Configuration;
using EasySynthesis.Api.Core.Responses;
using EasySynthesis.Infrastructure.Repositories;
using FastEndpoints;

namespace EasySynthesis.Api.Core.Login;

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

			// user = new User
			// {
			// 	Id = Guid.NewGuid(),
			// 	FirstName = "luki",
			// 	Type = UserType.HearingBooks
			// };
			var token = _userService.Authenticate(user);
			
			// var jwtToken = JWTBearer.CreateToken(
			// 	signingKey: _apiConfiguration.JwtSecret(),
			// 	expireAt: DateTime.UtcNow.AddDays(1),
			// 	claims: new[]
			// 	{
			// 		(ClaimNames.Email, user.Email), 
			// 		(ClaimNames.UserId, user.Id.ToString())
			// 	},
			// 	roles: new[] { user.Type.ToString() });

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