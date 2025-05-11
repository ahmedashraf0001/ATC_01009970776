using System.ComponentModel.DataAnnotations;

namespace event_booking_system.Common.Entites
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]      
        public string Name { get; set; }
        public ICollection<Event>? Events { get; set; }
    }
}
