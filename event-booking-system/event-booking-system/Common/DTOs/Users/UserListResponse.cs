namespace event_booking_system.Common.DTOs.Users
{
    public class UserListResponse
    {
        public List<UserProfileDTO> Users { get; set; }
        public int TotalCount { get; set; }
    }
}
