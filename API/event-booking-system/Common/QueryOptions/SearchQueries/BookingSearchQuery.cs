using event_booking_system.Common.Entites;
using System.ComponentModel.DataAnnotations;

namespace event_booking_system.Common.QueryOptions.SearchQueries
{
    public class BookingSearchQuery
    {
        [StringLength(200)]
        public string? keyword {  get; set; }
        public BookingStatus? Status { get; set; }
        public DateTime BookingDate { get; set; }

    }
}
