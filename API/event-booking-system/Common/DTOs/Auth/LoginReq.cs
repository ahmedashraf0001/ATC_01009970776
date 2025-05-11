using System.ComponentModel.DataAnnotations;

namespace event_booking_system.Common.DTOs.Auth
{
    public class LoginReq
    {
        [StringLength(100)]
        public string Username { get; set; }

        [Required(ErrorMessage = "New password is required.")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$",
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one digit.")]
        public string Password { get; set; }
    }
}
