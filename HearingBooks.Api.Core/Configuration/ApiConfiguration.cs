using Microsoft.Extensions.Configuration;

namespace HearingBooks.Api.Core.Configuration;

public class ApiConfiguration : IApiConfiguration
{
    private readonly IConfiguration _configuration;

    public string this[string key]
    {
        get => _configuration[key];
    }
    
    public ApiConfiguration(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string JwtSecret() 
        => _configuration.GetSection("Authorization")["Secret"];
}