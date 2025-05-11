using Event_ui.DTOs;
using Event_ui.DTOs.Bookings;
using Event_ui.DTOs.Events;
using Event_ui.DTOs.Users;
using Event_ui.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace Event_ui.Controllers
{
    public class BookingController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly ErrorsHandler errorsHandler;

        public BookingController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, ErrorsHandler errorsHandler)
        {
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _httpClient.BaseAddress = new Uri(_configuration["BaseUrl:Url"]);
            _httpContextAccessor = httpContextAccessor;
            this.errorsHandler = errorsHandler;
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Book(CreateBookingDTO request)
        {
            if (HttpClientHelper.AddAuthorizationHeader(_httpClient, _httpContextAccessor))
            {
                var jsonRequest = JsonConvert.SerializeObject(request);

                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("Booking/Book", content);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var booking = JsonConvert.DeserializeObject<BookingDTO>(json);
                    return View("BookingConfirmation", booking);
                }
                var errorResponse = await response.Content.ReadAsStringAsync();
                errorsHandler.HandleErrorResponse(response, errorResponse, TempData);

                return View("BookingConfirmation");
            }
            else
            {
                return RedirectToAction("Login", "Account");    
            }
        }
        [HttpGet]
        public async Task<IActionResult> Details(int BookingId)
        {
            if (HttpClientHelper.AddAuthorizationHeader(_httpClient, _httpContextAccessor))
            {
                var response = await _httpClient.GetAsync($"Booking/Id/{BookingId}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var booking = JsonConvert.DeserializeObject<BookingDTO>(json);
                    return View("BookingConfirmation", booking);
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

            var errorModel = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            return RedirectToAction("Error", "Home", errorModel);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UnBook(int BookingId)
        {
            if (HttpClientHelper.AddAuthorizationHeader(_httpClient, _httpContextAccessor))
            {

                var response = await _httpClient.DeleteAsync($"Booking/Unbook/{BookingId}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var booking = JsonConvert.DeserializeObject<BookingDTO>(json);
                    return View("UnBookingConfirmation", booking);
                }
                var errorResponse = await response.Content.ReadAsStringAsync();
                errorsHandler.HandleErrorResponse(response, errorResponse, TempData);

                return View("UnBookingConfirmation");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        [Authorize]
        public async Task<IActionResult> QuickBook(int EventId)
        {
            if (!HttpClientHelper.AddAuthorizationHeader(_httpClient, _httpContextAccessor))
                return RedirectToAction("Login", "Account");

            var eventResponse = await _httpClient.GetAsync($"Events/Id/{EventId}");
            if (eventResponse.IsSuccessStatusCode)
            {
                var eventJson = await eventResponse.Content.ReadAsStringAsync();
                var ev = JsonConvert.DeserializeObject<EventDetailsDTO>(eventJson);

                CreateBookingDTO request;
                if (ev.AdmissionTicketQty > 0)
                {
                    request = new CreateBookingDTO
                    {
                        EventId = EventId,
                        TicketType = TicketType.Admission,
                        AdmissionQty = 1,
                        VipQty = 0
                    };
                }
                else
                {
                    request = new CreateBookingDTO
                    {
                        EventId = EventId,
                        TicketType = TicketType.Vip,
                        AdmissionQty = 0,
                        VipQty = 1
                    };
                }
                var jsonRequest = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("Booking/Book", content);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var booking = JsonConvert.DeserializeObject<BookingDTO>(json);
                    return View("BookingConfirmation", booking);
                }
                var errorResponse = await response.Content.ReadAsStringAsync();
                errorsHandler.HandleErrorResponse(response, errorResponse, TempData);

                return View("BookingConfirmation");
            }

            var eventErrorResponse = await eventResponse.Content.ReadAsStringAsync();
            errorsHandler.HandleErrorResponse(eventResponse, eventErrorResponse, TempData);
            return View("BookingConfirmation");
        }

        [Authorize]
        public async Task<IActionResult> Panal(int pageNumber = 1, int pageSize = 5)
        {
            HttpClientHelper.AddAuthorizationHeader(_httpClient, _httpContextAccessor);
            var response = await _httpClient.GetAsync($"Booking/ListAll/{pageNumber}/{pageSize}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<BookingListResponse>(json);

                var model = new BookingPageViewModel
                {
                    Bookings = result.Bookings,
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
        public async Task<IActionResult> Cancel(int Id)
        {
            if (HttpClientHelper.AddAuthorizationHeader(_httpClient, _httpContextAccessor))
            {
                HttpClientHelper.AddAuthorizationHeader(_httpClient, _httpContextAccessor);
                var response = await _httpClient.DeleteAsync($"Booking/UnBook/{Id}");
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Panal", "Booking", new { pageNumber = 1, pageSize = 5 });
                }
                var errorResponse = await response.Content.ReadAsStringAsync();

                errorsHandler.HandleErrorResponse(response, errorResponse, TempData);

                return RedirectToAction("Panal", "Booking", new { pageNumber = 1, pageSize = 5 });
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        [Authorize]
        public async Task<IActionResult> CanceledDetails(int BookingId)
        {
            if (HttpClientHelper.AddAuthorizationHeader(_httpClient, _httpContextAccessor))
            {
                var response = await _httpClient.GetAsync($"Booking/Id/{BookingId}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var booking = JsonConvert.DeserializeObject<BookingDTO>(json);
                    return View("UnBookingConfirmation", booking);
                }
                var errorResponse = await response.Content.ReadAsStringAsync();

                errorsHandler.HandleErrorResponse(response, errorResponse, TempData);
                return RedirectToAction("Panal", "Booking", new { pageNumber = 1, pageSize = 5 });
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
    }
}
