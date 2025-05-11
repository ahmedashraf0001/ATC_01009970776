namespace event_booking_system.Common.DTOs.Bookings
{
    public class BookingListResponse
    {
        public List<BookingDTO> Bookings { get; set; }
        public int TotalCount { get; set; }
    }
}
