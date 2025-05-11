using System.ComponentModel.DataAnnotations;

namespace event_booking_system.Common.DTOs.Auth
{
    public class ForgotPassowrdDTO
    {
        [EmailAddress]
        public string Email { get; set; }
        public string returnUrl { get; set; }
    }
}
