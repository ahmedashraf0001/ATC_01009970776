﻿using System.ComponentModel.DataAnnotations;

namespace Event_ui.DTOs.Auth
{
    public class ResetPasswordDTO
    {
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "New password is required.")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one digit.")]
        public string? NewPassword { get; set; }

        public string Token { get; set; }
    }
    public class TokenPasswordDTO
    {
        [EmailAddress]
        public string Email { get; set; }

        public string Token { get; set; }
    }
}
