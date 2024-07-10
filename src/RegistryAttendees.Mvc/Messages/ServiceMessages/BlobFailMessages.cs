namespace RegistryAttendees.Mvc.Messages.ServiceMessages;

public static class BlobFailMessages
{
    public static string FailOnInsertToBlobStorage(string serviceName)
        => $"{serviceName}: : Problems while obtaining the container Instance, verify to cloud provider.";
    
    public static string ProblemWhileObtainingInstance(string serviceName)
        => $"{serviceName}: Problems while saving blobFile to Cloud BlobStorage.";
}