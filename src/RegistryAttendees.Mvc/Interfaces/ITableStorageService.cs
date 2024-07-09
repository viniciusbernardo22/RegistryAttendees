using Azure.Data.Tables;
using RegistryAttendees.Mvc.Entities;

namespace RegistryAttendees.Mvc.Interfaces;

public interface ITableStorageService<T> where T : class, ITableEntity
{
        Task<T> GetByIdAsync(string partitionKey,string identifier);

        Task<List<T>> GetAllAsync();

        Task UpsertAsync(T attendee);

        Task DeleteAsync(string partitionKey, string identifier);

}