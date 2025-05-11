using event_booking_system.Common.QueryOptions;

namespace event_booking_system.Services.Generic.Interfaces
{
    public interface IRepository<T>
    {
        Task<T?> GetByIdAsync(object id);
        Task<List<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task SaveAsync();
    }
}
