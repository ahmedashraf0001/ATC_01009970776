using Event_ui.DTOs.Bookings;

namespace Event_ui.DTOs.Users
{
    public class UserProfileDTO
    {
        public UserDetailDTO PersonalInfo { get; set; }
        public List<BookingDTO> Bookings { get; set; }
    }
}
