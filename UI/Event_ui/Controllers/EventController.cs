using Event_ui.DTOs;
using Event_ui.DTOs.Events;
using Event_ui.DTOs.Users;
using Event_ui.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;

namespace Event_ui.Controllers
{
    public class EventController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly ErrorsHandler errorsHandler;

        public EventController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, ErrorsHandler errorsHandler)
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
            var response = await _httpClient.GetAsync($"Events/List/{pageNumber}/{pageSize}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<EventListResponse>(json);

                var model = new EventPageViewModel
                {
                    Events = result.Events,
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
        public async Task<IActionResult> Index()
        {
            HttpClientHelper.AddAuthorizationHeader(_httpClient, _httpContextAccessor);
            var response = await _httpClient.GetAsync($"Events/List/{1}/{9}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<EventListResponse>(json);
                return View(result.Events);
            }

            var errorModel = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            return RedirectToAction("Error", "Home", errorModel);
        }
        public async Task<IActionResult> All(int pageNumber = 1, int pageSize = 12)
        {
            HttpClientHelper.AddAuthorizationHeader(_httpClient, _httpContextAccessor);
            var response = await _httpClient.GetAsync($"Events/List/{pageNumber}/{pageSize}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<EventListResponse>(json);

                var model = new EventPageViewModel
                {
                    Events = result.Events,
                    TotalCount = result.TotalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };

                return View(model);
            }

            var errorModel = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            return RedirectToAction("Error", "Home", errorModel);
        }
        public async Task<IActionResult> Details(int id)
        {
            HttpClientHelper.AddAuthorizationHeader(_httpClient, _httpContextAccessor);
            var response = await _httpClient.GetAsync($"Events/Id/{id}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var events = JsonConvert.DeserializeObject<EventDetailsDTO>(json);
                return View(events);
            }

            var errorModel = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            return RedirectToAction("Error", "Home", errorModel);
        }
        public async Task<IActionResult> Search(string query, int pageNumber = 1, int pageSize = 9)
        {
            HttpClientHelper.AddAuthorizationHeader(_httpClient, _httpContextAccessor);

            var url = $"Events/Search?Keyword={query}&pageNumber={pageNumber}&pageSize={pageSize}";

            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<EventListResponse>(json);

                var model = new EventPageViewModel
                {
                    Events = result.Events,
                    TotalCount = result.TotalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    searchQuery = query
                };

                return View("All", model);
            }

            var errorModel = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            return RedirectToAction("Error", "Home", errorModel);
        }
        public MultipartFormDataContent MapToEditForm(UpdateEventDTO request)
        {
           var formContent = new MultipartFormDataContent();

            if (!string.IsNullOrWhiteSpace(request.Title))
                formContent.Add(new StringContent(request.Title), nameof(request.Title));

            if (!string.IsNullOrWhiteSpace(request.Description))
                formContent.Add(new StringContent(request.Description), nameof(request.Description));

            if (!string.IsNullOrWhiteSpace(request.Category))
                formContent.Add(new StringContent(request.Category), nameof(request.Category));

            if (request.Date != default)
                formContent.Add(new StringContent(request.Date.Value.ToString("o")), nameof(request.Date));

            if (!string.IsNullOrWhiteSpace(request.Location))
                formContent.Add(new StringContent(request.Location), nameof(request.Location));

            if (request.VipPrice.HasValue)
                formContent.Add(new StringContent(request.VipPrice.Value.ToString()), nameof(request.VipPrice));

            if (request.AdmissionPrice.HasValue)
                formContent.Add(new StringContent(request.AdmissionPrice.Value.ToString()), nameof(request.AdmissionPrice));

            if (request.AdmissionTicketQty.HasValue)
                formContent.Add(new StringContent(request.AdmissionTicketQty.Value.ToString()), nameof(request.AdmissionTicketQty));

            if (request.VipTicketQty.HasValue)
                formContent.Add(new StringContent(request.VipTicketQty.Value.ToString()), nameof(request.VipTicketQty));

            if (request.file != null && request.file.Length > 0)
            {
                var streamContent = new StreamContent(request.file.OpenReadStream());
                formContent.Add(streamContent, nameof(request.file), request.file.FileName);
            }
            return formContent;
        }
        
        public MultipartFormDataContent MapToCreateForm(CreateEventDTO request)
        {
            var formContent = new MultipartFormDataContent();

            formContent.Add(new StringContent(request.Title), nameof(request.Title));

            formContent.Add(new StringContent(request.Description), nameof(request.Description));

            formContent.Add(new StringContent(request.Category), nameof(request.Category));

            formContent.Add(new StringContent(request.Date.ToString("o")), nameof(request.Date));

            formContent.Add(new StringContent(request.Location), nameof(request.Location));

            formContent.Add(new StringContent(request.VipPrice.ToString()), nameof(request.VipPrice));

            formContent.Add(new StringContent(request.AdmissionPrice.ToString()), nameof(request.AdmissionPrice));

            formContent.Add(new StringContent(request.AdmissionTicketQty.ToString()), nameof(request.AdmissionTicketQty));

            formContent.Add(new StringContent(request.VipTicketQty.ToString()), nameof(request.VipTicketQty));

            if (request.file != null && request.file.Length > 0)
            {
                var streamContent = new StreamContent(request.file.OpenReadStream());
                formContent.Add(streamContent, nameof(request.file), request.file.FileName);
            }
            return formContent;
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateEventDTO request)
        {
            HttpClientHelper.AddAuthorizationHeader(_httpClient, _httpContextAccessor);

            var formContent = MapToEditForm(request);

            var response = await _httpClient.PutAsync($"Events/Edit/{request.Id}", formContent);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Panal", "Event", new { pageNumber = 1,  pageSize = 5 });
            }

            var errorResponse = await response.Content.ReadAsStringAsync();
            errorsHandler.HandleErrorResponse(response, errorResponse, TempData);

            return RedirectToAction("Panal", "Event", new { pageNumber = 1, pageSize = 5 });
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(CreateEventDTO request)
        {
            HttpClientHelper.AddAuthorizationHeader(_httpClient, _httpContextAccessor);

            var formContent = MapToCreateForm(request);

            var response = await _httpClient.PostAsync($"Events/Create", formContent);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Panal", "Event", new { pageNumber = 1, pageSize = 5 });
            }

            var errorResponse = await response.Content.ReadAsStringAsync();
            errorsHandler.HandleErrorResponse(response, errorResponse, TempData);

            return RedirectToAction("Panal", "Event", new { pageNumber = 1, pageSize = 5 });
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(int eventId)
        {
            HttpClientHelper.AddAuthorizationHeader(_httpClient, _httpContextAccessor);

            var response = await _httpClient.DeleteAsync($"Events/Delete/{eventId}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Panal", "Event", new { pageNumber = 1, pageSize = 5 });
            }
            var errorResponse = await response.Content.ReadAsStringAsync();

            errorsHandler.HandleErrorResponse(response, errorResponse, TempData);

            return RedirectToAction("Panal", "Event", new { pageNumber = 1, pageSize = 5 });
        }
    }
}
