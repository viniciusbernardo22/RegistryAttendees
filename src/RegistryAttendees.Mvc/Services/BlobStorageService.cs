using Azure.Storage.Blobs;
using RegistryAttendees.Mvc.Extensions;
using RegistryAttendees.Mvc.Interfaces;

namespace RegistryAttendees.Mvc.Services;

public class BlobStorageService : IBlobStorageService
{
    private const string TableName = "atendeeimages";

    private readonly IConfiguration _configuration;
    public BlobStorageService(IConfiguration configuration)
    {
        _configuration = configuration;
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
            throw;
        }
    }
    
    
}