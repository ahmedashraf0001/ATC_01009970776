using event_booking_system.Common.Entites;
using event_booking_system.Common.QueryOptions.IncludeQueries;
using event_booking_system.Common.QueryOptions.SearchQueries;
using event_booking_system.Services.Generic.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Hosting;

namespace event_booking_system.Repositories.Interfaces
{
    public interface IEventRepository : IRepository<Event>
    {
        Task<List<Event>> GetByUserIdAsync(string userId, EventQuery options = null);
        Task<(List<Event>, int)> SearchEventsAsync(EventSearchQuery searchQuery, int pageNumber, int pageSize = 12);
        Task<Event?> GetByIdAsync(int id, EventQuery options = null);
        Task<int> GetCount();
        Task<List<Event>> recentEvents(int windowSize);
        Task<(List<Event>, int)> GetListAsync(int pageNumber, int pageSize = 12);
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<List<Event>> GetEventsByCategory(int CategoryId);
    }
}
