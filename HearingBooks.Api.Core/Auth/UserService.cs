using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HearingBooks.Api.Core.Configuration;
using HearingBooks.Api.Core.TimeProvider;
using HearingBooks.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace HearingBooks.Api.Core.Auth;

public class UserService : IUserService
{
    private readonly IApiConfiguration _configuration;
    private readonly ITimeProvider _timeProvider;

    public UserService(IApiConfiguration configuration, ITimeProvider timeProvider)
    {
        _configuration = configuration;
        _timeProvider = timeProvider;
    }

    public string Authenticate(User user)
    {
        return GenerateJwtToken(user);
    }

    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var secret = _configuration.JwtSecret();
        var key = Encoding.ASCII.GetBytes(secret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
                new[]
                {
                    new Claim("id", user.Id.ToString()),
                    new Claim("firstName", user.FirstName),
                    new Claim("lastName", user.LastName),
                    new Claim("email", user.Email),
                    new Claim("role", user.Type.ToString())
                }
            ),

            //TODO: Change that back to 1 hour
            Expires = _timeProvider.UtcNow().UtcDateTime.AddMonths(1),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}