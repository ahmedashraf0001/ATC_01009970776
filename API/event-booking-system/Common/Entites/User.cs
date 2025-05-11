using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace event_booking_system.Common.Entites
{
    public enum RoleType
    {
        Admin,
        User      
    }
    public class User:IdentityUser
    {
        [StringLength(50)]
        public string? FirstName { get; set; }
        [StringLength(50)]
        public string? LastName { get; set; }
        [Phone]
        public string? PhoneNumber {  get; set; }

        [StringLength(200)]
        public string? Bio {  get; set; }
        public string? ImageUrl { get; set; }
        [StringLength(50)]
        public RoleType Role { get; set; } = RoleType.User;
        [StringLength(200)]
        public string Address { get; set; }

        public DateTime DateJoined { get; set; } = DateTime.UtcNow;
        public ICollection<Booking>? Bookings { get; set; }
        public ICollection<Event>? CreatedEvents { get; set; } 
    }
}
