using System.ComponentModel.DataAnnotations;

namespace event_booking_system.Common.Validations
{
    public class FileExtension: ValidationAttribute
    {
        private readonly string[] _extensions;

        public FileExtension(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName).ToLower();
                if (!_extensions.Contains(extension))
                {
                    return new ValidationResult($"Allowed file types: {string.Join(", ", _extensions)}");
                }
            }

            return ValidationResult.Success!;
        }
    }
}
