using Azure;
using Azure.Data.Tables;
using RegistryAttendees.Mvc.Entities;
using RegistryAttendees.Mvc.Interfaces;

namespace RegistryAttendees.Mvc.Services;

public class TableStorageService<T> : ITableStorageService<T> where T : class, ITableEntity
{
    private const string TableName = $"{nameof(T)}s";
    
    private readonly IConfiguration _configuration;
    
        public TableStorageService(IConfiguration configuration)
        => _configuration = configuration;
        
    private async Task<TableClient> GetTableClient()
    {
        var serviceClient = new TableServiceClient(_configuration["StorageConnectionString"]);
        var tableClient = serviceClient.GetTableClient(TableName);
        /*Create table if not exists*/
        await tableClient.CreateIfNotExistsAsync();
        return tableClient;
    }
    
    public async Task<T> GetByIdAsync(string industry, string id)
    {
        var tableClient = await GetTableClient();
        return await tableClient.GetEntityAsync<T>(industry, id);
    }
    
    public async Task<List<T>> GetAllAsync()
    {
        var tableClient = await GetTableClient();
        return tableClient.Query<T>().ToList();
    }

    public async Task UpsertAsync(T attendee)
    {
        var tableClient = await GetTableClient(); 
        await tableClient.UpsertEntityAsync(attendee);
    }

    public async Task DeleteAsync(string industry, string id)
    {
        var tableClient = await GetTableClient();

        await tableClient.DeleteEntityAsync(industry, id);
    }
}