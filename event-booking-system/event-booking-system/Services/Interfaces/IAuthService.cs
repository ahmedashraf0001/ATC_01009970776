using event_booking_system.Common.DTOs.Auth;

namespace event_booking_system.Services.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterReq registerDto);
        Task<TokenResponse> LoginAsync(LoginReq loginDto);
        Task<TokenResponse> CreateToken(TokenDTO tokenDTO);
        Task ForgotPasswordAsync(ForgotPassowrdDTO model);
        Task ResetPasswordAsync(ResetPasswordDTO model);
        string GenerateResetLink(string frontendResetPasswordUrlBase, string token, string userEmail);
        Task InitAdminAccount();
    }
}
