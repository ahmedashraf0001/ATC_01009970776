using event_booking_system.Common.DTOs.Auth;
using event_booking_system.Common.Entites;
using event_booking_system.Common.Utils;
using event_booking_system.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace event_booking_system.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AuthService(UserManager<User> userManager, IConfiguration configuration, IEmailService emailService, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _configuration = configuration;
            _emailService = emailService;
            _roleManager = roleManager;
        }

        public async Task<TokenResponse> CreateToken(TokenDTO tokenDTO)
        {
            var user = await _userManager.FindByNameAsync(tokenDTO.Username);
            if (user == null)
                throw new NotFoundException("User not found");

            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                new Claim(ClaimTypes.NameIdentifier, user.Id ?? string.Empty),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("Image", user.ImageUrl ?? "/uploads/default.png"),
                new Claim(ClaimTypes.Role, (await _userManager.GetRolesAsync(user)).FirstOrDefault())
            };


            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var accessToken = GenerateToken(authClaims);
            var expiration = DateTime.UtcNow.AddHours(3);

            return new TokenResponse
            {
                AccessToken = accessToken,
                Exp_date = expiration
            };
        }

        public async Task ForgotPasswordAsync(ForgotPassowrdDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                throw new NotFoundException($"Email: {model.Email} not found");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            if (token == null)
                throw new BadRequestException("Failed to generate password token, please try again later");

            var link = GenerateResetLink(model.returnUrl, token, model.Email);
            if (string.IsNullOrEmpty(link))
                throw new BadRequestException("Base reset URL is missing or invalid");

            await _emailService.SendResetAsync(model.Email, user.UserName, link);
        }

        public string GenerateResetLink(string frontendResetPasswordUrlBase, string token, string userEmail)
        {
            if (frontendResetPasswordUrlBase == null)
                return string.Empty;

            var encodedToken = HttpUtility.UrlEncode(token);
            return $"{frontendResetPasswordUrlBase}?email={HttpUtility.UrlEncode(userEmail)}&token={encodedToken}";
        }

        public async Task ResetPasswordAsync(ResetPasswordDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                throw new NotFoundException($"Email: {model.Email} not found");

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new ValidationException($"Failed to reset password. Errors: {errors}");
            }
        }

        public async Task<TokenResponse> LoginAsync(LoginReq loginDto)
        {
            loginDto.Username = loginDto.Username.Replace(" ", "_");

            var user = await _userManager.FindByNameAsync(loginDto.Username);
            if (user == null)
                throw new UnauthorizedException("Invalid username or password");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isPasswordValid)
                throw new UnauthorizedException("Invalid username or password");

            var tokenDto = new TokenDTO { Username = user.UserName };
            return await CreateToken(tokenDto);
        }

        public async Task RegisterAsync(RegisterReq registerDto)
        {
            var userExists = await _userManager.FindByNameAsync(registerDto.Fullname);
            if (userExists != null)
                throw new ConflictException("A user with this username already exists");

            var emailExists = await _userManager.FindByEmailAsync(registerDto.Email);
            if (emailExists != null)
                throw new ConflictException("A user with this email already exists");

            var nameParts = Regex.Split(registerDto.Fullname, @"[_\s]+", RegexOptions.Compiled | RegexOptions.CultureInvariant);

            User user = new User()
            {
                UserName = registerDto.Fullname.Replace(" ", "_"),
                FirstName = nameParts.FirstOrDefault(),
                LastName = nameParts.Length > 1 ? string.Join(" ", nameParts.Skip(1)) : string.Empty,
                Email = registerDto.Email,
                Address = registerDto.Address,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new ValidationException($"User creation failed: {errors}");
            }
            var roleResult = await _userManager.AddToRoleAsync(user, "User");

        }

        private string GenerateToken(IEnumerable<Claim> claims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["JWT:ValidIssuer"],
                Audience = _configuration["JWT:ValidAudience"],
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(claims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task InitAdminAccount()
        {
            const string adminRole = "Admin";
            const string adminEmail = "admin@example.com";
            const string adminPassword = "Admin@123";

            try
            {
                if (!await _roleManager.RoleExistsAsync(adminRole))
                {
                    var roleResult = await _roleManager.CreateAsync(new IdentityRole(adminRole));
                    if (!roleResult.Succeeded)
                        throw new Exception($"Failed to create role '{adminRole}': {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
                }

                var adminUser = await _userManager.FindByEmailAsync(adminEmail);
                if (adminUser == null)
                {
                    var newAdmin = new User
                    {
                        UserName = "Admin",
                        FirstName = "Admin",
                        Email = adminEmail,
                        EmailConfirmed = true,
                        Address = "Default"
                    };

                    var createResult = await _userManager.CreateAsync(newAdmin, adminPassword);
                    if (!createResult.Succeeded)
                        throw new Exception($"Failed to create admin user: {string.Join(", ", createResult.Errors.Select(e => e.Description))}");

                    await _userManager.AddToRoleAsync(newAdmin, adminRole);
                    await _userManager.AddClaimAsync(newAdmin, new Claim(ClaimTypes.Role, adminRole));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error seeding admin: {ex.Message}");
                throw;
            }
        }
    }
}
