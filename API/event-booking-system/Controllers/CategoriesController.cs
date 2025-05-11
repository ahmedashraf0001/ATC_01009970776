using event_booking_system.Common.DTOs.Categories;
using event_booking_system.Common.Entites;
using event_booking_system.Common.Utils;
using event_booking_system.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace event_booking_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpPost("Create")]
        public async Task<ActionResult<CategoryDTO>> Create(CategoryReq Category)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                throw new ValidationException(errors);
            }
            var model = await _categoryService.CreateCategoryAsync(Category.Name);
            return Ok(model);
        }
        [HttpDelete("Delete/{Categoryname}")]
        public async Task<ActionResult> Delete(string Categoryname)
        {
            await _categoryService.DeleteCategoryAsync(Categoryname);
            return Ok("Deleted Sucessfully!");
        }
        [HttpPut("RemoveFrom/{eventId}")]
        public async Task<ActionResult> RemoveFrom(int eventId, CategoryReq Category)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                throw new ValidationException(errors);
            }
            await _categoryService.RemoveFromCategoryAsync(eventId, Category.Name);
            return Ok("Removed Sucessfully!");
        }
        [HttpPut("AddTo/{eventId}")]
        public async Task<ActionResult> AddTo(int eventId, CategoryReq Category)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                throw new ValidationException(errors);
            }
            await _categoryService.AddToCategoryAsync(eventId, Category.Name);
            return Ok("Added Sucessfully!");
        }
        [HttpPut("Update/{oldName}")]
        public async Task<ActionResult<CategoryDTO>> Update(string oldName, CategoryReq Category)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                throw new ValidationException(errors);
            }
            var model = await _categoryService.UpdateCategoryNameAsync(Category.Name, oldName);
            return Ok(model);
        }
        [HttpGet("List/{pageNumber:int}/{pageSize:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<List<CategoryListResponse>>> List(int pageNumber, int pageSize = 12)
        {
            var model = await _categoryService.GetAllAsync(pageNumber, pageSize);
            var result = new CategoryListResponse
            {
                Categories = model.Item1,
                TotalCount = model.Item2
            };
            return Ok(result);
        }
        [HttpGet("Id/{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<CategoryDTO>> ByID(int id)
        {
            var model = await _categoryService.GetByIdAsync(id);
            return Ok(model);
        }
    }
}
