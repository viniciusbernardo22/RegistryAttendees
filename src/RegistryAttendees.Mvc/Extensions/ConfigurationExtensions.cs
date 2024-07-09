namespace RegistryAttendees.Mvc.Extensions
{
    public static class ConfigurationExtensions 
    {
        public static string GetAzureStorageConnectionString(this IConfiguration configuration)
            => configuration["StorageConnectionString"];
        
        
    }
}