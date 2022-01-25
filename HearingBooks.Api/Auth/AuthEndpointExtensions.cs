using Api.Factories;
using Contracts.Responses;
using HearingBooks.Api.Syntheses;
using HearingBooks.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HearingBooks.Api.Auth;

public static class AuthEndpointExtensions
{
	private static readonly string _baseEndpointGroupRoute = "auth";

	public static void MapAuthEndpoints(this WebApplication app)
	{
		app.MapPost(
			$"/{_baseEndpointGroupRoute}/login", 
			async (
				[FromServices] IUserService userService,
				[FromServices] IUserRepository userRepository,
				[FromBody] AuthenticateUserDto request) =>
			{
				try
				{
					if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
					{
						var errorMessage = "Email and Password have to be provided!";
						throw new ArgumentException(errorMessage);
					}
            
					// var user = await _dbContext.Set<User>()
					// 	.SingleOrDefaultAsync(u => u.Email.Equals(request.Email)
					// 	                           && u.Password.Equals(request.Password));

					var user = await userRepository.GetUserByCredentialsAsync(request.Email, request.Password);

					if (user == null)
					{
						var errorMessage = "User with provided credentials do not exist!";
						throw new ArgumentException(errorMessage);
					}

					var token = userService.Authenticate(user);
            
					return ResponseFactory.CreateSuccessResponse(new AuthenticateUserResponse {Token = token});
				}
				catch (ArgumentException e)
				{
					return ResponseFactory.CreateFailureResponse<AuthenticateUserResponse>(e.Message);
				}
			});
	}
}