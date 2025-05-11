using System.ComponentModel.DataAnnotations;

namespace event_booking_system.Common.DTOs.Categories
{
    public class CategoryReq
    {
        [StringLength(50)]

        public string Name { get; set; }
    }
}
