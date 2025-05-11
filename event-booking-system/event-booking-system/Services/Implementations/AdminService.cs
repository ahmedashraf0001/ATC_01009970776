using event_booking_system.Common.DTOs.Others;
using event_booking_system.Services.Interfaces;

namespace event_booking_system.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly IEventService _eventService;
        private readonly IUserService _userService;
        private readonly IBookingService _bookingService;
        public AdminService(IEventService eventService, IUserService userService, IBookingService bookingService)
        {
            _eventService = eventService;
            _userService = userService;
            _bookingService = bookingService;
        }
        public async Task<DashboardDTO> dashboard()
        {
            return new DashboardDTO
            {
                TotalEvents = await _eventService.GetTotalEventsCount(),
                UsersCount = await _userService.GetUsersCount(),
                TotalBookings = await _bookingService.GetBookingCount(),
                Revenue = await _bookingService.GetRevenue(),
                recentEvents = await _eventService.recentEvents(4)
            };
        }
    }
}
