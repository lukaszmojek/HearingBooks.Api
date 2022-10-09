using HearingBooks.Services.Core.Storage;
using HearingBooks.Tests.Core.Helpers;

namespace HearingBooks.Api.Tests.Helpers;

public static class StorageHelpers
{
    public static IStorageService CreateStorageService()
    {
        var apiConfiguration = ConfigurationHelpers.CreateConfiguration();

        return new StorageService(apiConfiguration);
    }
}