namespace event_booking_system.Common.DTOs.Auth
{
    public class TokenResponse
    {
        public string AccessToken { get; set; }
        public DateTime Exp_date { get; set; }
    }
}
