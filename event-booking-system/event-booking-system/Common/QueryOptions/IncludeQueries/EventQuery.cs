namespace event_booking_system.Common.QueryOptions.IncludeQueries
{
    public class EventQuery
    {
        public bool WithUserCreated { get; set; } = false;
        public bool WithCategory { get; set; } = false;
        public bool WithBookings { get; set; } = false;
    }
}
