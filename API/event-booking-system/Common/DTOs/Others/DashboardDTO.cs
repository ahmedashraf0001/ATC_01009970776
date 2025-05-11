using event_booking_system.Common.DTOs.Events;

namespace event_booking_system.Common.DTOs.Others
{
    public class DashboardDTO
    {
        public int TotalEvents { get; set; }
        public int UsersCount { get; set; }
        public int TotalBookings { get; set; }
        public decimal Revenue { get; set; }

        public List<EventDashboardDTO> recentEvents { get; set; }
    }
}
