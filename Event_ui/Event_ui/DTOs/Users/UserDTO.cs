using System.ComponentModel.DataAnnotations;

namespace Event_ui.DTOs.Users
{
    public enum RoleType
    {
        Admin,
        User
    }
    public class UserDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public RoleType Role { get; set; }
        public DateTime DateJoined { get; set; }
    }
    public class UserEditDTO
    {
        public string Id { get; set; }
        [StringLength(100)]
        public string? Name { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        [StringLength(200)]
        public string? Address { get; set; }
        [Phone]
        public string? Phone { get; set; }
        [StringLength(200)]
        public string? Bio { get; set; }
        public RoleType? Role { get; set; }

        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$",
    ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one digit.")]
        public string? CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$",
    ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one digit.")]
        public string? NewPassword { get; set; }

        public IFormFile? file { get; set; }
    }
}
