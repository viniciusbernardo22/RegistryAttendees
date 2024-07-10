using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using RegistryAttendees.Mvc.Extensions;
using RegistryAttendees.Mvc.Interfaces;
using RegistryAttendees.Mvc.Messages.ServiceMessages;

namespace RegistryAttendees.Mvc.Services;

public class BlobStorageService : IBlobStorageService
{
    private const string TableName = "atendeeimages";

    private readonly IConfiguration _configuration;

    private readonly ILogger _logger;
    public BlobStorageService(ILogger logger, IConfiguration configuration)
    {
        _configuration = configuration;
        _logger = logger;
    }

    private async Task<BlobContainerClient> GetBlobContainerClient()
    {
        try
        {
            BlobContainerClient container = new BlobContainerClient(_configuration.GetAzureStorageConnectionString(), TableName);
            await container.CreateIfNotExistsAsync();

            return container;
        }
        catch(Exception ex)
        {
            _logger.LogWarning(BlobFailMessages.ProblemWhileObtainingInstance(nameof(BlobStorageService)));
            throw;
        }
    }
    
    public async Task<string> UploadBlob(IFormFile formFile, string imageName)
    {
        try
        {
            var blobName = $"{imageName}{Path.GetExtension(formFile.FileName)}";
            var container = await GetBlobContainerClient();
            using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            
            await container.UploadBlobAsync(blobName, memoryStream);
            return blobName;
        }
        catch (Exception e)
        {
            _logger.LogWarning(BlobFailMessages.FailOnInsertToBlobStorage(nameof(BlobStorageService)));
            throw;
        }
       
    }

    public async Task<string> GetBlobUrl(string imageName)
    {
        try
        {
            var container = await GetBlobContainerClient();

            var blob = container.GetBlobClient(imageName);

            BlobSasBuilder blobSasBuilder = new()
            {
                BlobContainerName = blob.BlobContainerName,
                BlobName = blob.Name,
                ExpiresOn = DateTime.UtcNow.AddMinutes(2),
                Protocol = SasProtocol.Https,
                Resource = "b"
            };
            blobSasBuilder.SetPermissions(BlobAccountSasPermissions.Read);
            return blob.GenerateSasUri(blobSasBuilder).ToString();
        }
        catch (Exception e)
        {
            _logger.LogWarning(BlobFailMessages.ProblemWhileGettingBlobUrl(nameof(BlobStorageService)));
            throw;
        }
        
    }

    public async Task RemoveBlob(string imageName)
    {
        var container = await GetBlobContainerClient();

        var blob = container.GetBlobClient(imageName);

        await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
    }
}