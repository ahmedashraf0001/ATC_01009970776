using System.ComponentModel.DataAnnotations;

namespace event_booking_system.Common.Validations
{
    public class MaxSize : ValidationAttribute
    {
        private readonly int _maxSize;

        public MaxSize(int maxSize)
        {
            _maxSize = maxSize;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file && file.Length > _maxSize)
            {
                return new ValidationResult($"File size cannot exceed {(_maxSize / (1024 * 1024))}MB.");
            }
            return ValidationResult.Success!;
        }
    }
}
