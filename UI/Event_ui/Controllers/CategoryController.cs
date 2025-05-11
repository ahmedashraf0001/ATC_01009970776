using Event_ui.DTOs;
using Event_ui.DTOs.Categories;
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
    public class CategoryController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly ErrorsHandler errorsHandler;

        public CategoryController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, ErrorsHandler errorsHandler)
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
            var response = await _httpClient.GetAsync($"Categories/List/{pageNumber}/{pageSize}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<CategoryListResponse>(json);

                var model = new CategoryPageViewModel
                {
                    Categories = result.Categories,
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
        [HttpPost]
        public async Task<IActionResult> Edit(string request, string id)
        {
            HttpClientHelper.AddAuthorizationHeader(_httpClient, _httpContextAccessor);

            var content = new CategoryReq { Name = request };

            var jsonContent = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"Categories/Update/{id}", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Panal", "Category", new { pageNumber = 1, pageSize = 5 });
            }

            var errorResponse = await response.Content.ReadAsStringAsync();
            errorsHandler.HandleGeneralErrorResponse(response, errorResponse, TempData);

            return RedirectToAction("Panal", "Category", new { pageNumber = 1, pageSize = 5 });
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(string name)
        {
            HttpClientHelper.AddAuthorizationHeader(_httpClient, _httpContextAccessor);

            var content = new CategoryReq { Name = name };

            var jsonContent = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"Categories/Create", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Panal", "Category", new { pageNumber = 1, pageSize = 5 });
            }
            var errorResponse = await response.Content.ReadAsStringAsync();
            errorsHandler.HandleGeneralErrorResponse(response, errorResponse, TempData);


            return RedirectToAction("Panal", "Category", new { pageNumber = 1, pageSize = 5 });
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(string name)
        {
            HttpClientHelper.AddAuthorizationHeader(_httpClient, _httpContextAccessor);

            var response = await _httpClient.DeleteAsync($"Categories/Delete/{name}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Panal", "Category", new { pageNumber = 1, pageSize = 5 });
            }

            var errorResponse = await response.Content.ReadAsStringAsync();
            errorsHandler.HandleGeneralErrorResponse(response, errorResponse, TempData);


            return RedirectToAction("Panal", "Category", new { pageNumber = 1, pageSize = 5 });
        }
    }
}
