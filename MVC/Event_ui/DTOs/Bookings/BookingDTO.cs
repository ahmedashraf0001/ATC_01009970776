using System.Net.Sockets;

namespace Event_ui.DTOs.Bookings
{
    public enum TicketType
    {
        Admission,
        Vip,
        Both
    }
    public class BookingDTO
    {
        public int Id { get; set; }
        public int EvId { get; set; }
        public string EvTitle { get; set; }
        public string EvImageUrl { get; set; }
        public string EvCategory { get; set; }
        public int AdmissionTicketQty { get; set; }
        public int VipTicketQty { get; set; }
        public decimal AdmissionPrice { get; set; }
        public decimal VipPrice { get; set; }
        public DateTime EvDate { get; set; }
        public string EvLocation { get; set; }
        public string userName { get; set; }
        public TicketType TicketType { get; set; }
        public DateTime BookingDate { get; set; }
        public BookingStatus Status { get; set; }
        public decimal EvPrice { get; set; }
    }
}
