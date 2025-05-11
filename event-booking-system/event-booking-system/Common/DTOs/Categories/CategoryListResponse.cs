namespace event_booking_system.Common.DTOs.Categories
{
    public class CategoryListResponse
    {
        public List<CategoryDTO> Categories { get; set; }
        public int TotalCount { get; set; }
    }
}
