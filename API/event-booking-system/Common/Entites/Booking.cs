using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace event_booking_system.Common.Entites
{
    public enum BookingStatus
    {
        Pending,    
        Confirmed,  
        Canceled    
    }
    public enum TicketType
    {
        Admission,
        Vip,
        Both
    }
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }

        public int EventId { get; set; }
        [ForeignKey(nameof(EventId))]
        public Event? Event { get; set; }

        public int AdmissionTicketQty { get; set; }
        public int VipTicketQty { get; set; }
        public TicketType TicketType { get; set; }
        public DateTime BookingDate { get; set; } = DateTime.Now;
        public BookingStatus Status { get; set; }

    }
}
