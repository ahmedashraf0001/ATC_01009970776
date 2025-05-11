using Microsoft.AspNetCore.Mvc.Formatters;

namespace event_booking_system.Common.Utils
{
    public class FileUpload
    {
        private readonly IConfiguration _configuration;
        public FileUpload(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> UploadAsync(IFormFile? file)
        {
            if (file == null || file.Length == 0)
                return ("N/A");

            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), _configuration["Uploads:Route"]);

            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            var extension = Path.GetExtension(file.FileName).ToLower();
            var fileName = Guid.NewGuid() + extension;
            var filePath = Path.Combine(uploadsPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var mediaUrl = $"/uploads/{fileName}";

            return mediaUrl;
        }

        public async Task<string> DeleteAsync(string mediaUrl)
        {
            if (string.IsNullOrEmpty(mediaUrl))
                return "Invalid media URL.";

            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), _configuration["Uploads:Route"]);

            var fileName = mediaUrl.Replace("/uploads/", "");
            var filePath = Path.Combine(uploadsPath, fileName);

            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                    return "File deleted successfully.";
                }
                catch (Exception ex)
                {
                    return $"Error deleting file: {ex.Message}";
                }
            }

            return "File not found.";
        }
    }
}
