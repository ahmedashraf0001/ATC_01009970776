namespace Event_ui.DTOs.Events
{
    public class EventPageViewModel
    {
        public List<EventDTO> Events { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string searchQuery { get; set; }
    }
}
