namespace Event_ui.DTOs.Events
{
    public class EventListResponse
    {
        public List<EventDTO> Events { get; set; }
        public int TotalCount { get; set; }
    }
}
