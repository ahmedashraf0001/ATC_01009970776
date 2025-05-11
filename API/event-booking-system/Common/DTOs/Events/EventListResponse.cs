namespace event_booking_system.Common.DTOs.Events
{
    public class EventListResponse
    {
        public List<EventDTO> Events { get; set; }
        public int TotalCount { get; set; }
    }

}
