using Azure.Data.Tables;
using RegistryAttendees.Mvc.Interfaces;

namespace RegistryAttendees.Mvc.Services;

public class TableStorageService<T> : ITableStorageService<T>
{
    private const string TableName = $"{nameof(T)}s";
    
    private readonly IConfiguration _configuration;

    private async Task<TableClient> GetTableClient()
    {
        var serviceClient = new TableServiceClient(_configuration["StorageConnectionString"]);
        var tableClient = serviceClient.GetTableClient(TableName);
        /*Create table if not exists*/
        await tableClient.CreateIfNotExistsAsync();
        return tableClient;
    }
    
    public TableStorageService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task<T> GetById(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<T>> GetAll()
    {
        throw new NotImplementedException();
    }

    public async Task Upsert(T attendee)
    {
        throw new NotImplementedException();
    }

    public async Task Delete(string? id)
    {
        throw new NotImplementedException();
    }
}