namespace Event_ui.DTOs.Users
{
    public class UserPageViewModel
    {
        public List<UserDetailDTO> Users { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string searchQuery { get; set; }
    }
}
