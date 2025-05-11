using Event_ui.DTOs.Others;
using Event_ui.DTOs.Users;
using Event_ui.Util;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Event_ui.Controllers
{
    public class AdminController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        public AdminController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _httpClient.BaseAddress = new Uri(_configuration["BaseUrl:Url"]);
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> Panal()
        {
            HttpClientHelper.AddAuthorizationHeader(_httpClient, _httpContextAccessor);
            var response = await _httpClient.GetAsync($"Admin/Dashboard");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var events = JsonConvert.DeserializeObject<DashboardDTO>(json);
                return View(events);
            }

            var errorModel = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            return RedirectToAction("Error", "Home", errorModel);
        }
    }
}
