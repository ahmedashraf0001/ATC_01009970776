using event_booking_system.Common.Entites;
using System.ComponentModel.DataAnnotations;

namespace event_booking_system.Common.DTOs.Users
{
    public class UserProfileDTO
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
        public string? ImageUrl { get; set; }
    }
}
