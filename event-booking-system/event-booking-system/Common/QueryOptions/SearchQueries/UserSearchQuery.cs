using event_booking_system.Common.Entites;
using System.ComponentModel.DataAnnotations;

namespace event_booking_system.Common.QueryOptions.SearchQueries
{
    public class UserSearchQuery
    {
        [StringLength(200)]

        public string? Keyword {  get; set; }
        public DateTime DateJoined { get; set; }

        public RoleType? RoleType { get; set; }
    }
}
