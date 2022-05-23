using EasySynthesis.Api.Storage;

namespace EasySynthesis.Api.Tests.Helpers;

public static class StorageHelpers
{
    public static IStorageService CresteStorageService()
    {
        var apiConfiguration = ConfigurationHelpers.CreateConfiguration();

        return new StorageService(apiConfiguration);
    }
}