using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;

namespace Event_ui.DTOs.Bookings
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
