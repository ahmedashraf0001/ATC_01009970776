using event_booking_system.Common.Entites;
using event_booking_system.Services.Generic.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace event_booking_system.Repositories.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category> GetByNameAsync(string name, bool withEvents = false);
        Task<(List<Category>, int)> GetAllAsync(int pageNumber, int pageSize = 12);
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
