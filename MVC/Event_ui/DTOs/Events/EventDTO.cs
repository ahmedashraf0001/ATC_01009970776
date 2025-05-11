namespace Event_ui.DTOs.Events
{
    public class EventDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public decimal VipPrice { get; set; }
        public decimal AdmissionPrice { get; set; }
        public int AdmissionTicketQty { get; set; }
        public int VipTicketQty { get; set; }
        public bool SoldOut { get; set; }
        public bool IsBooked { get; set; }
    }
}
