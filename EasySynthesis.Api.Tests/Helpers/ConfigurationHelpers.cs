using EasySynthesis.Api.Core.Configuration;
using Microsoft.Extensions.Configuration;

namespace EasySynthesis.Api.Tests.Helpers;

public static class ConfigurationHelpers
{
    public static IApiConfiguration CreateConfiguration()
    {
        var configuration = new ConfigurationManager()
            .AddJsonFile("appsettings.Development.json")
            .Build();

        return new ApiConfiguration(configuration);
    }
}