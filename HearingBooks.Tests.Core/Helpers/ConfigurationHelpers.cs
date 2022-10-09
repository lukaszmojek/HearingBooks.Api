using Microsoft.Extensions.Configuration;

namespace HearingBooks.Tests.Core.Helpers;

public static class ConfigurationHelpers
{
    public static IConfiguration CreateConfiguration()
    {
        var configuration = new ConfigurationManager()
            .AddJsonFile("appsettings.Development.json")
            .Build();

        return configuration;
    }
}