using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using Event_ui.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using Event_ui.DTOs.Auth;
using Event_ui.Util;

namespace Event_ui.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ErrorsHandler errorsHandler;
        public AccountController(IHttpClientFactory httpClientFactory, IConfiguration configuration, ErrorsHandler errorsHandler)
        {
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _httpClient.BaseAddress = new Uri(_configuration["BaseUrl:Url"]);
            this.errorsHandler = errorsHandler;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO request)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpClient.PostAsJsonAsync("Auth/login", request);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var tokenContainer = JsonConvert.DeserializeObject<TokenResponse>(json);
                    var expirationTime = tokenContainer.Exp_date;

                    CookieOptions cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = expirationTime
                    };
                    Response.Cookies.Append("JWT", tokenContainer.AccessToken, cookieOptions);
                    Response.Cookies.Append("JWT_Expiration", expirationTime.ToString("o"), cookieOptions);

                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadJwtToken(tokenContainer.AccessToken);

                    var claims = new List<Claim>();

                    var nameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "unique_name" || c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
                    if (nameClaim != null)
                    {
                        claims.Add(new Claim(ClaimTypes.Name, nameClaim.Value));
                    }

                    var idClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid");
                    if (idClaim != null)
                    {
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, idClaim.Value));
                    }

                    var roleClaims = jwtToken.Claims.Where(c => c.Type == "role" || c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
                    foreach (var roleClaim in roleClaims)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, roleClaim.Value));
                    }

                    var pictureClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "Image");
                    if (pictureClaim != null)
                    {
                        claims.Add(new Claim("Image", pictureClaim.Value));
                    }

                    var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "email" || c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
                    if (emailClaim != null)
                    {
                        claims.Add(new Claim(ClaimTypes.Email, emailClaim.Value));
                    }

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        principal,
                        new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = expirationTime
                        });

                    return RedirectToAction("Index", "Event", new { pageNumber = 1 });
                }
                var errorResponse = await response.Content.ReadAsStringAsync();
                errorsHandler.HandleGeneralErrorWithInjectionResponse(response, errorResponse, ModelState);
            }
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .Distinct()
                .ToList();
            foreach (var error in errors)
            {
                ModelState.AddModelError(string.Empty, error); 
            }

            return View(request);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            Response.Cookies.Delete("JWT");
            Response.Cookies.Delete("JWT_Expiration");

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO request)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpClient.PostAsJsonAsync("Auth/register", request);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Login");
                }

                var errorResponse = await response.Content.ReadAsStringAsync();
                errorsHandler.HandleGeneralErrorWithInjectionResponse(response, errorResponse, ModelState);
            }
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .Distinct()
                .ToList();
            foreach (var error in errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> Reset()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Reset(ForgotPassowrdDTO request)
        {
            if (ModelState.IsValid)
            {
                request.returnUrl = "https://localhost:7124/Account/returnReset";
                var response = await _httpClient.PostAsJsonAsync("Auth/forgot-password", request);
                if (response.IsSuccessStatusCode)
                {
                    return View("PasswordResetConf", request);

                }

                var errorResponse = await response.Content.ReadAsStringAsync();
                errorsHandler.HandleGeneralErrorWithInjectionResponse(response, errorResponse, ModelState);

            }
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .Distinct()
                .ToList();
            foreach (var error in errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> returnReset(TokenPasswordDTO request)
        {
              return View("PasswordReset", request);
        }
        [HttpPost]
        public async Task<IActionResult> returnReset(ResetPasswordDTO request)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpClient.PostAsJsonAsync("Auth/reset-password", request);
                if (response.IsSuccessStatusCode)
                {
                    return View("PasswordReseted", request);

                }
                var errorResponse = await response.Content.ReadAsStringAsync();
                errorsHandler.HandleGeneralErrorWithInjectionResponse(response, errorResponse, ModelState);

            }
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .Distinct()
                .ToList();
            foreach (var error in errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
            return View("PasswordReset", request);
        }
    }
}