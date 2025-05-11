namespace Event_ui.DTOs.Bookings
{
    public class BookingPageViewModel
    {
        public List<BookingDTO> Bookings { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string searchQuery { get; set; }
    }
}
