using event_booking_system.Common.Entites;
using event_booking_system.Common.QueryOptions.IncludeQueries;
using event_booking_system.Common.QueryOptions.SearchQueries;
using event_booking_system.Services.Generic.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace event_booking_system.Repositories.Interfaces
{
    public interface IBookingRepository : IRepository<Booking>
    {
        Task<Booking> GetByIdAsync(int id, BookingQuery options = null);
        Task<List<Booking>> GetByUserIdAsync(string userId, BookingQuery options, int pageNumber, int pageSize = 12);
        Task<Booking> GetByUserAndEventAsync(string userId, int eventId, BookingQuery options = null);
        Task<(List<Booking>, int)> GetAllAsync(int pageNumber, int pageSize = 12, BookingQuery options = null);
        Task<List<Booking>> SearchBookingsAsync(BookingSearchQuery searchQuery, int pageNumber, int pageSize = 12);
        Task<List<Booking>> GetBookingsByEventIdAsync(int eventId, BookingQuery options, int pageNumber, int pageSize = 12);
        Task<int> GetCount();
        Task<decimal> GetRevenue();
        Task<int> GetBookingCount(int eventId);
        Task<bool> IsBooked(string userId, int eventId);
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
