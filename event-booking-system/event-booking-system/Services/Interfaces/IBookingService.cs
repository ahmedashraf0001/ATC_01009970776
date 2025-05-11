using event_booking_system.Common.DTOs.Bookings;
using event_booking_system.Common.Entites;
using event_booking_system.Common.QueryOptions.IncludeQueries;
using event_booking_system.Common.QueryOptions.SearchQueries;
using MimeKit.Tnef;

namespace event_booking_system.Services.Interfaces
{
    public interface IBookingService
    {
        Task<BookingDTO> GetByIdAsync(int id);
        Task<(List<BookingDTO>, int)> ListAllBookings(int pageNumber, int pageSize = 12);
        Task<List<BookingDTO>> GetBookingsByUserIdAsync(string userId, int pageNumber, int pageSize = 12);
        Task<bool> IsBooked(string userId, int eventId);
        Task<BookingDTO> BookEventAsync(CreateBookingDTO request, string userId);
        Task<List<BookingDTO>> SearchBookingsAsync(BookingSearchQuery searchQuery, int pageNumber, int pageSize = 12);
        Task<BookingDTO> ChangeStatus(BookingStatus status, int bookingId);
        Task<BookingDTO> UnBookingAsync(int id);
        Task<int> GetBookingCount();
        Task<decimal> GetRevenue();
        Task<int> GetBookingCount(int eventId);
        Task<List<BookingDTO>> GetBookingsByEventIdAsync(int eventId, BookingQuery options, int pageNumber, int pageSize = 12);
    }
}
