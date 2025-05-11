namespace event_booking_system.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendResetAsync(string toEmail, string userName, string resetLink);
    }
}
