namespace Event_ui.DTOs.Others
{
    public class DashboardDTO
    {
        public int TotalEvents { get; set; }
        public int UsersCount { get; set; }
        public int TotalBookings { get; set; }
        public decimal Revenue { get; set; }

        public List<EventDashboardDTO> recentEvents { get; set; }
    }
    public class EventDashboardDTO
    {
        public string EvTitle { get; set; }
        public DateTime EvDate { get; set; }

        public string EvCategory { get; set; }
        public int numOfBooking { get; set; }
    }
}
