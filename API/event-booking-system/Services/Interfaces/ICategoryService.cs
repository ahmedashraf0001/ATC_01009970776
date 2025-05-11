using event_booking_system.Common.DTOs.Categories;
using event_booking_system.Common.Entites;

namespace event_booking_system.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<Category> GetByNameAsync(string name, bool withEvents = false);
        Task<CategoryDTO> CreateCategoryAsync(string name);
        Task AddToCategoryAsync(int eventId, string name);
        Task RemoveFromCategoryAsync(int eventId, string name);
        Task DeleteCategoryAsync(string name);
        Task<CategoryDTO> UpdateCategoryNameAsync(string newName, string oldName);
        Task<CategoryDTO> GetByIdAsync(int id);
        Task<(List<CategoryDTO>, int)> GetAllAsync(int pageNumber, int pageSize = 12);
    }
}
