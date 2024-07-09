using RegistryAttendees.Mvc.Entities;

namespace RegistryAttendees.Mvc.Interfaces;

public interface ITableStorageService<T>
{
        Task<T> GetById(string id);

        Task<List<T>> GetAll();

        Task Upsert(T attendee);

        Task Delete(string? id);

}