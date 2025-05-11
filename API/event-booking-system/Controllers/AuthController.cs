using event_booking_system.Common.DTOs.Auth;
using event_booking_system.Common.Utils;
using event_booking_system.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterReq registerDto)
    {
        if (!ModelState.IsValid)
        {
            var errors = string.Join(", ", ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));
            throw new ValidationException(errors);
        }

        if (registerDto.Password != registerDto.ConfPass)
            throw new ValidationException("Passwords do not match.");

        await _authService.RegisterAsync(registerDto);
        return Ok(new { Message = "User registered successfully" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginReq loginDto)
    {
        if (!ModelState.IsValid)
        {
            var errors = string.Join(", ", ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));
            throw new ValidationException(errors);
        }

        var result = await _authService.LoginAsync(loginDto);
        return Ok(result);
    }


    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(ForgotPassowrdDTO model)
    {
        if (!ModelState.IsValid)
        {
            var errors = string.Join(", ", ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));
            throw new ValidationException(errors);
        }
        await _authService.ForgotPasswordAsync(model);
        return Ok(new { Message = "We've sent you a password reset email!" });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordDTO model)
    {
        if (!ModelState.IsValid)
        {
            var errors = string.Join(", ", ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));
            throw new ValidationException(errors);
        }
        await _authService.ResetPasswordAsync(model);
        return Ok(new { Message = "Password reset successfully!" });
    }
}
