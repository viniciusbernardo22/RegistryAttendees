using Azure.Storage.Blobs;

namespace RegistryAttendees.Mvc.Interfaces;

public interface IBlobStorageService
{
    Task<string> UploadBlob(IFormFile formFile, string imageName);
    
}