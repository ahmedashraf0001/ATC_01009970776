using event_booking_system.Common.DTOs.Events;
using event_booking_system.Common.Entites;
using event_booking_system.Common.QueryOptions.SearchQueries;

namespace event_booking_system.Services.Interfaces
{
    public interface IEventService
    {
        Task<(List<EventDTO>, int)> GetEventsPageAsync(string userId, string CurrentUserId, int pageNumber, int pageSize = 12); 
        Task<EventDetailsDTO> GetEventDetailsAsync(int id, string userId);
        Task<(List<EventDTO>, int)> SearchEventsAsync(string userId, EventSearchQuery searchQuery, string CurrentUserId, int pageNumber, int pageSize = 12);
        Task<EventDTO> CreateEventAsync(CreateEventDTO request, string adminId);
        Task<EventDTO> UpdateEventAsync(int eventId, UpdateEventDTO request, string CurrentUserId);
        Task<bool> DeleteEventAsync(int eventId);
        Task<List<EventDashboardDTO>> recentEvents(int windowSize);
        Task<int> GetTotalEventsCount();
    }
}
