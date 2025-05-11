using System.ComponentModel.DataAnnotations;

namespace Event_ui.DTOs.Users
{
    public class UserDetailDTO
    {
        public string Id { get; set; }
        [StringLength(50)]
        public string? FirstName { get; set; }
        [StringLength(50)]
        public string? LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Address { get; set; }
        [StringLength(200)]
        public string? Bio { get; set; }
        public RoleType? Role { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }
        public string? CurrentPassword { get; set; }

        public string? ImageUrl { get; set; }
    }
}
