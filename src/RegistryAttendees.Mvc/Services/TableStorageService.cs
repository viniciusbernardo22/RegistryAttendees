using Azure;
using Azure.Data.Tables;
using RegistryAttendees.Mvc.Entities;
using RegistryAttendees.Mvc.Extensions;
using RegistryAttendees.Mvc.Interfaces;
using ConfigurationExtensions = RegistryAttendees.Mvc.Extensions.ConfigurationExtensions;


namespace RegistryAttendees.Mvc.Services;

public class TableStorageService<T> : ITableStorageService<T> where T : class, ITableEntity
{
    private const string TableName = "Atendees";

    private readonly IConfiguration _configuration;
    public TableStorageService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    private async Task<TableClient> GetTableClient()
    {
        var serviceClient = new TableServiceClient(_configuration.GetAzureStorageConnectionString());
        
        var tableClient = serviceClient.GetTableClient(TableName);
        /*Create table if not exists*/
        await tableClient.CreateIfNotExistsAsync();
        return tableClient;
    }
    
    public async Task<T> GetByIdAsync(string partitionKey, string identifier)
    {
        var tableClient = await GetTableClient();
        return await tableClient.GetEntityAsync<T>(partitionKey, identifier);
    }
    
    public async Task<List<T>> GetAllAsync()
    {
        var tableClient = await GetTableClient();
        return tableClient.Query<T>().ToList();
    }

    public async Task UpsertAsync(T entity)
    {
        var tableClient = await GetTableClient(); 
        await tableClient.UpsertEntityAsync(entity);
    }

    public async Task DeleteAsync(string partitionKey, string identifier)
    {
        var tableClient = await GetTableClient();

        await tableClient.DeleteEntityAsync(partitionKey, identifier);
    }
}