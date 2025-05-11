namespace Event_ui.DTOs.Users
{
    public class UserListResponse
    {
        public List<UserDetailDTO> Users { get; set; }
        public int TotalCount { get; set; }
    }
}
