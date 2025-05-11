namespace event_booking_system.Common.DTOs.Events
{
    public class EventDashboardDTO
    {
        public string EvTitle { get; set; }
        public DateTime EvDate { get; set; }

        public string EvCategory { get; set; }
        public int numOfBooking { get; set; }
    }
}
