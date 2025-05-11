using System.ComponentModel.DataAnnotations;
using event_booking_system.Common.Validations;
namespace event_booking_system.Common.DTOs.Events
{
    public class CreateEventDTO
    {
        [StringLength(50)]
        public string Title { get; set; }
        [StringLength(200)]
        public string Description { get; set; }
        public string? Category { get; set; }
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
        [FileExtension(new[] { ".png", ".jpg", ".jpeg", ".mp4", ".gif" })]
        [MaxSize(50 * 1024 * 1024)]
        public IFormFile? file { get; set; }
    }
    public class UpdateEventDTO
    {
        [StringLength(50)]

        public string? Title { get; set; }
        [StringLength(200)]
        public string? Description { get; set; }
        [StringLength(50)]
        public string? Category { get; set; }
        public DateTime? Date { get; set; }
        [StringLength(100)]

        public string? Location { get; set; }
        [Range(0, int.MaxValue)]

        public decimal? VipPrice { get; set; }
        [Range(0, int.MaxValue)]

        public decimal? AdmissionPrice { get; set; }
        [Range(0, int.MaxValue)]

        public int? AdmissionTicketQty { get; set; }
        [Range(0, int.MaxValue)]

        public int? VipTicketQty { get; set; }

        [FileExtension(new[] { ".png", ".jpg", ".jpeg", ".mp4", ".gif" })]
        [MaxSize(50 * 1024 * 1024)]
        public IFormFile? file { get; set; }
    }
}
