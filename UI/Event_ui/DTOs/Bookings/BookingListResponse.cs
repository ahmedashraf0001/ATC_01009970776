
namespace Event_ui.DTOs.Bookings
{
    public class BookingListResponse
    {
        public List<BookingDTO> Bookings { get; set; }
        public int TotalCount { get; set; }
    }
}
