namespace event_booking_system.Common.QueryOptions.IncludeQueries
{
    public class BookingQuery
    {
        public bool WithEvent { get; set; } = false;
        public bool WithUser { get; set; } = false;

    }
}
