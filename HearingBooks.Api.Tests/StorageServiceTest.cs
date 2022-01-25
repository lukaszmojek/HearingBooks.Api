using System;
using System.Threading;
using System.Threading.Tasks;
using HearingBooks.Api.Configuration;
using HearingBooks.Api.Storage;
using HearingBooks.Tests.Core;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace HearingBooks.Api.Tests;

public class StorageServiceTest {
     public class StorageServiceTestBase : TestBase
     {
         protected readonly string _containerName = Guid.NewGuid().ToString();
         protected IStorageService StorageService;
         
         public async Task RunAsync()
         {
             await PrepareEnvironmentForTest();
             await ExecuteTestAsync();
             await Cleanup();
         }

         public virtual Task ExecuteTestAsync()
         {
             return Task.CompletedTask;
         }
    
         private async Task PrepareEnvironmentForTest()
         {
             var configuration = new ConfigurationManager()
                 .AddJsonFile("appsettings.Development.json")
                 .Build();
    
             var apiConfiguration = new ApiConfiguration(configuration);
    
             StorageService = new StorageService(apiConfiguration);

             await DeleteContainerIfExists();
         }
    
         private async Task Cleanup()
         {
             await DeleteContainerIfExists();
         }
         
         private async Task DeleteContainerIfExists()
         {
             if (await StorageService.ContainerExistsAsync(_containerName))
             {
                 await StorageService.DeleteContainerAsync(_containerName);
             }
         }
     }
     
    public class When_ContainerExistsAsync_Is_Called_Should_Return_False_When_Container_Does_Not_Exists : StorageServiceTestBase
    {
        //TODO: Think how this can be refactored
        [Fact]
        public async Task InvokeTest()
        {
            await RunAsync();
        }
        
        public override async Task ExecuteTestAsync()
        {
            var containerExistsBeforeCallingCreateContainer = await StorageService.ContainerExistsAsync(_containerName);
            var _ = await StorageService.CreateContainerAsync(_containerName);
            var containerExistsAfterCallingCreateContainer = await StorageService.ContainerExistsAsync(_containerName);
    
            Assert.False(containerExistsBeforeCallingCreateContainer);
            Assert.True(containerExistsAfterCallingCreateContainer);
        }
    }

    public class When_ContainerExistsAsync_Is_Called_Should_Return_True_When_Container_Exists : StorageServiceTestBase
    {
        [Fact]
        public async Task InvokeTest()
        {
            await RunAsync();
        }
        
        public override async Task ExecuteTestAsync()
        {
            await StorageService.CreateContainerAsync(_containerName);
            var containerExists = await StorageService.ContainerExistsAsync(_containerName);
    
            Assert.True(containerExists);
        }
    }

    public class When_CreateContainer_Is_Called_Should_Create_Container_In_AzureBlobStorage : StorageServiceTestBase
    {
        [Fact]
        public async Task InvokeTest()
        {
            await RunAsync();
        }
        
        public override async Task ExecuteTestAsync()
        {
            var containerExistsBeforeCallingCreateContainer = await StorageService.ContainerExistsAsync(_containerName);
            var _ = await StorageService.CreateContainerAsync(_containerName);
            var containerExistsAfterCallingCreateContainer = await StorageService.ContainerExistsAsync(_containerName);
            
            Assert.False(containerExistsBeforeCallingCreateContainer);
            Assert.True(containerExistsAfterCallingCreateContainer);
        }
    }

    public class When_UploadBlob_Is_Called_Should_Upload_Blob_To_Container_In_AzureBlobStorage : StorageServiceTestBase
    {
        private readonly string _fileName = "opowiesci-niesamowite.epub";
        
        [Fact]
        public async Task InvokeTest()
        {
            await RunAsync();
        }
        
        public override async Task ExecuteTestAsync()
        {
            var blobContainerClient = await StorageService.CreateContainerAsync(_containerName);
            var containerExistsAfterCallingCreateContainer = await StorageService.ContainerExistsAsync(_containerName);
            var _ = await StorageService.UploadBlobAsync(
                blobContainerClient,
                _fileName,
                GetPathForTestFile(_fileName)
            );
    
            var blobExists = await StorageService.BlobExistsAsync(
                blobContainerClient,
                _fileName,
                CancellationToken.None
            );
            
            Assert.True(containerExistsAfterCallingCreateContainer);
            Assert.True(blobExists);
        }
    }
    
    public class When_DeleteContainerAsync_Is_Called_Should_Delete_Container_From_AzureBlobStorage : StorageServiceTestBase
    {
        [Fact]
        public async Task InvokeTest()
        {
            await RunAsync();
        }
        
        public override async Task ExecuteTestAsync()
        {
            var _ = await StorageService.CreateContainerAsync(_containerName);
            var containerExistsBeforeCallingDeleteContainer = await StorageService.ContainerExistsAsync(_containerName);
            await StorageService.DeleteContainerAsync(_containerName);
            var containerExistsAfterCallingDeleteContainer = await StorageService.ContainerExistsAsync(_containerName);

            Assert.True(containerExistsBeforeCallingDeleteContainer);
            Assert.False(containerExistsAfterCallingDeleteContainer);
        }
    }
}