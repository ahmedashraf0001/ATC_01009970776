using Event_ui.DTOs;
using Event_ui.DTOs.Bookings;
using Event_ui.DTOs.Users;
using Event_ui.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Event_ui.Controllers
{
    public class UserController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly ErrorsHandler errorsHandler;

        public UserController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, ErrorsHandler errorsHandler)
        {
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient("ApiClient"); 
            _httpClient.BaseAddress = new Uri(_configuration["BaseUrl:Url"]);
            _httpContextAccessor = httpContextAccessor;
            this.errorsHandler = errorsHandler;
        }
        [Authorize]
        public async Task<IActionResult> Panal(int pageNumber = 1, int pageSize = 5)
        {
            HttpClientHelper.AddAuthorizationHeader(_httpClient, _httpContextAccessor);
            var response = await _httpClient.GetAsync($"User/List/{pageNumber}/{pageSize}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<UserListResponse>(json);
                var model = new UserPageViewModel
                {
                    Users = result.Users,
                    TotalCount = result.TotalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };

                errorsHandler.InjectErrorMessages(TempData, ModelState);

                return View(model);
            }
            else
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                try
                {
                    var error = JsonConvert.DeserializeObject<ApiErrorResponse>(errorResponse);
                    var errorModel = new ErrorViewModel
                    {
                        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                        ErrorMessage = error?.Message ?? "An error occurred while fetching the user list."
                    };
                    return RedirectToAction("Error", "Home", errorModel);
                }
                catch
                {
                    var errorModel = new ErrorViewModel
                    {
                        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                        ErrorMessage = "An error occurred while fetching the user list."
                    };
                    return RedirectToAction("Error", "Home", errorModel);
                }
            }
        }
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            HttpClientHelper.AddAuthorizationHeader(_httpClient, _httpContextAccessor);
            var response = await _httpClient.GetAsync($"User/Details");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<UserDetailDTO>(json);
                return View(result);
            }

            var errorModel = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            return RedirectToAction("Error", "Home", errorModel);
        }
        [Authorize]
        public async Task<IActionResult> Security()
        {
            HttpClientHelper.AddAuthorizationHeader(_httpClient, _httpContextAccessor);
            var response = await _httpClient.GetAsync($"User/Details");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<UserDetailDTO>(json);
                return View(result);
            }

            var errorModel = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            return RedirectToAction("Error", "Home", errorModel);

        }
        public MultipartFormDataContent MapToForm(UserEditDTO request)
        {
            var formContent = new MultipartFormDataContent();

            formContent.Add(new StringContent(request.Id), nameof(request.Id));
            if (!string.IsNullOrWhiteSpace(request.Name))
                formContent.Add(new StringContent(request.Name), nameof(request.Name));
            if (!string.IsNullOrWhiteSpace(request.Email))
                formContent.Add(new StringContent(request.Email), nameof(request.Email));
            if (!string.IsNullOrWhiteSpace(request.Address))
                formContent.Add(new StringContent(request.Address), nameof(request.Address));
            if (!string.IsNullOrWhiteSpace(request.Phone))
                formContent.Add(new StringContent(request.Phone), nameof(request.Phone));
            if (!string.IsNullOrWhiteSpace(request.Bio))
                formContent.Add(new StringContent(request.Bio), nameof(request.Bio));
            if (!string.IsNullOrWhiteSpace(request.NewPassword))
                formContent.Add(new StringContent(request.NewPassword), nameof(request.NewPassword));
            if (!string.IsNullOrWhiteSpace(request.CurrentPassword))
                formContent.Add(new StringContent(request.CurrentPassword), nameof(request.CurrentPassword));
            if (request.Role.HasValue)
                formContent.Add(new StringContent(((int)request.Role.Value).ToString()), nameof(request.Role));

            if (request.file != null && request.file.Length > 0)
            {
                var streamContent = new StreamContent(request.file.OpenReadStream());
                formContent.Add(streamContent, nameof(request.file), request.file.FileName);
            }
            return formContent;
        }
        [Authorize]
        public async Task<IActionResult> Bookings(int pageNumber = 1, int pageSize=12)
        {
            HttpClientHelper.AddAuthorizationHeader(_httpClient, _httpContextAccessor);
            var response1 = await _httpClient.GetAsync($"User/Details");
            var response2 = await _httpClient.GetAsync($"Booking/List/Current/{pageNumber}/{pageSize}");

            if (response1.IsSuccessStatusCode && response2.IsSuccessStatusCode)
            {
                var json1 = await response1.Content.ReadAsStringAsync();
                var json2 = await response2.Content.ReadAsStringAsync();

                UserProfileDTO result = new UserProfileDTO()
                {
                    PersonalInfo = JsonConvert.DeserializeObject<UserDetailDTO>(json1),
                    Bookings = JsonConvert.DeserializeObject<List<BookingDTO>>(json2)
                };               
                return View(result);
            }

            var errorModel = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            return RedirectToAction("Error", "Home", errorModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UserEditDTO request)
        {
            HttpClientHelper.AddAuthorizationHeader(_httpClient, _httpContextAccessor);
            var formContent = MapToForm(request);

            var response = await _httpClient.PutAsync("User/Edit", formContent);

            var current = TempData["Current"] as string;
            bool isProfile = string.Equals(current, "Profile", StringComparison.OrdinalIgnoreCase);
            bool isSecurity = string.Equals(current, "Security", StringComparison.OrdinalIgnoreCase);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<UserDetailDTO>(json);


                if (User.IsInRole("Admin"))
                {
                    if (isProfile)
                        return View("Profile", result);

                    if (isSecurity)
                    {
                        TempData["PasswordChanged"] = true;
                        return View("Security", result);
                    }

                    return RedirectToAction("Panal", "User", new { pageNumber = 1, pageSize = 5 });
                }
                else
                {
                    if (isProfile)
                        return View("Profile", result);

                    TempData["PasswordChanged"] = true;
                    return View("Security", result);
                }
            }


            var errorResponse = await response.Content.ReadAsStringAsync();

            errorsHandler.HandleErrorResponse(response, errorResponse, TempData);

            if (User.IsInRole("Admin"))
            {
                if (isProfile)
                    return View("Profile");

                if (isSecurity)
                {
                    return View("Security");
                }

                return RedirectToAction("Panal", "User", new { pageNumber = 1, pageSize = 5 });
            }
            else
            {
                if (isProfile)
                    return View("Profile");

                return View("Security");
            }

        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(string Id)
        {
            HttpClientHelper.AddAuthorizationHeader(_httpClient, _httpContextAccessor);

            var response = await _httpClient.DeleteAsync($"User/Delete/{Id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Panal", new { pageNumber = 1, pageSize = 5 });
            }
            var errorResponse = await response.Content.ReadAsStringAsync();

            errorsHandler.HandleGeneralErrorResponse(response, errorResponse, TempData);
            return RedirectToAction("Panal", "User", new { pageNumber = 1, pageSize = 5 });
        }
   
    }
}
