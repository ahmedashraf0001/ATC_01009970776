namespace event_booking_system.Common.DTOs.Events
{
    public class EventDetailsDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public decimal VipPrice { get; set; }
        public decimal AdmissionPrice { get; set; }
        public int AdmissionTicketQty { get; set; }
        public int VipTicketQty { get; set; }
        public string ImageUrl { get; set; }
        public string CreatedBy { get; set; }
        public bool IsBooked { get; set; }

    }
}
