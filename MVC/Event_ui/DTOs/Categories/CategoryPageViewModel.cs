namespace Event_ui.DTOs.Categories
{
    public class CategoryPageViewModel
    {
        public List<CategoryDTO> Categories { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string searchQuery { get; set; }
    }
}
