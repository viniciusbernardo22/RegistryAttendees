using Azure.Storage.Blobs;

namespace RegistryAttendees.Mvc.Interfaces;

public interface IBlobStorageService
{
    Task<string> UploadBlob(IFormFile formFile, string imageName);
    
    Task<string> GetBlobUrl(string imageName);
    
     Task RemoveBlob(string imageName);
}