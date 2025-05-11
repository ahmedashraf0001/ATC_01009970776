using System.ComponentModel.DataAnnotations;

namespace Event_ui.DTOs.Auth
{
    public class ForgotPassowrdDTO
    {
        [EmailAddress]
        public string Email { get; set; }
        public string? returnUrl { get; set; }

    }
}
