namespace Event_ui.DTOs.Bookings
{
    public enum BookingStatus
    {
        Pending,
        Confirmed,
        Canceled
    }
    public class BookingSearchQuery
    {
        public string? keyword { get; set; }
        public BookingStatus? Status { get; set; }
        public DateTime BookingDate { get; set; }
    }
}
