namespace event_booking_system.Common.QueryOptions.IncludeQueries
{
    public class UserQuery
    {
        public bool WithBookings { get; set; } = false;
        public bool WithCreatedEv { get; set; } = false;
    }
}
