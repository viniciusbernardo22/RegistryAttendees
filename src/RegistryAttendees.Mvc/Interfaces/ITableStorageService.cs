using Azure.Data.Tables;
using RegistryAttendees.Mvc.Entities;

namespace RegistryAttendees.Mvc.Interfaces;

public interface ITableStorageService<T> where T : class, ITableEntity
{
        Task<T> GetByIdAsync(string industry,string id);

        Task<List<T>> GetAllAsync();

        Task UpsertAsync(T attendee);

        Task DeleteAsync(string industry, string id);

}