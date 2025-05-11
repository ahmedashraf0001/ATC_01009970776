using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace event_booking_system.Common.Entites
{
    public class Event
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        public string Title { get; set; }    
        [StringLength(200)]
        public string Description { get; set; } 
        public DateTime Date { get; set; }
        [StringLength(100)]

        public string Location { get; set; }   
        [Range(0, int.MaxValue)]
        public decimal VipPrice { get; set; }   
        [Range(0, int.MaxValue)]
        public decimal AdmissionPrice { get; set; }
        [Range(0, int.MaxValue)]

        public int AdmissionTicketQty { get; set; }
        [Range(0, int.MaxValue)]

        public int VipTicketQty { get; set; }
        public string ImageUrl { get; set; }
        public string CreatedById { get; set; }

        [ForeignKey(nameof(CreatedById))]
        public User? CreatedBy { get; set; } 

        public int? CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category? Category { get; set; }  

        public ICollection<Booking>? Bookings { get; set; }
    }

}
