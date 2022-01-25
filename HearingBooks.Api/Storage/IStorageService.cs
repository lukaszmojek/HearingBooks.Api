using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace HearingBooks.Api.Storage;

public interface IStorageService
{
    Task<bool> ContainerExistsAsync(string containerName);
    
    Task<BlobContainerClient> GetBlobContainerClientAsync(string containerName);
    
    Task<BlobContainerClient> CreateContainerAsync(string containerName);
    
    Task DeleteContainerAsync(string containerName);

    Task<Response<BlobContentInfo>> UploadBlobAsync(
        BlobContainerClient blobContainerClient, string blobFileName, string blobLocalPath
    );

    Task<Response<bool>> BlobExistsAsync(
        BlobContainerClient blobContainerClient, string blobFileName, CancellationToken cancellationToken
    );
}