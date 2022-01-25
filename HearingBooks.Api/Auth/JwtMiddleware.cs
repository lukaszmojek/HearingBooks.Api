using System.IdentityModel.Tokens.Jwt;
using System.Text;
using HearingBooks.Api.Configuration;
using HearingBooks.Infrastructure.Repositories;
using Microsoft.IdentityModel.Tokens;

#pragma warning disable CS1591

namespace HearingBooks.Api.Auth;

public partial class JwtMiddleware
{
    private readonly IApiConfiguration _configuration;
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next, IApiConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task Invoke(HttpContext context, IUserRepository userRepository)
    {
        var token = context.Request
            .Headers["Authorization"]
            .FirstOrDefault()?
            .Split(" ")
            .Last();

        if (token != null)
        {
            await AttachUserToContext(context, userRepository, token);
        }

        await _next(context);
    }

    private async Task AttachUserToContext(HttpContext context, IUserRepository userRepository, string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.JwtSecret());

            tokenHandler.ValidateToken(
                token,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                },
                out var validatedToken
            );

            var jwtToken = (JwtSecurityToken) validatedToken;
            var userId = Guid.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

            context.Items["User"] = await userRepository.GetUserByIdAsync(userId);
        }
        catch (Exception e)
        {
            LogDetails(e);
            throw new AuthorizationException(e.Message);
        }
    }
}