using event_booking_system.Common.Entites;
using System.ComponentModel.DataAnnotations;

namespace event_booking_system.Common.DTOs.Bookings
{
    public class CreateBookingDTO
    {
        public int EventId { get; set; }
        public TicketType TicketType { get; set; }
        [Range(0, int.MaxValue)]

        public int? VipQty { get; set; }
        [Range(0, int.MaxValue)]

        public int? AdmissionQty { get; set; }
    }
}
