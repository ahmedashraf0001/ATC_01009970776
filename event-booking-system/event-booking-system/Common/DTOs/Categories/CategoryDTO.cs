namespace event_booking_system.Common.DTOs.Categories
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int EventCount { get; set; }
        public List<string> Event_List { get; set; }
    }
}
