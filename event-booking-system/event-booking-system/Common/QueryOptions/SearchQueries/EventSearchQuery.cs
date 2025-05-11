using System.ComponentModel.DataAnnotations;

namespace event_booking_system.Common.QueryOptions.SearchQueries
{
    public class EventSearchQuery
    {
        [StringLength(200)]

        public string? Keyword { get; set; }
        public DateTime? Date { get; set; }
        [Range(0, int.MaxValue)]
        public decimal? Price { get; set; }
    }
}