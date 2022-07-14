using EasySynthesis.Services.Core.Storage;
using EasySynthesis.Tests.Core.Helpers;

namespace EasySynthesis.Api.Tests.Helpers;

public static class StorageHelpers
{
    public static IStorageService CresteStorageService()
    {
        var apiConfiguration = ConfigurationHelpers.CreateConfiguration();

        return new StorageService(apiConfiguration);
    }
}