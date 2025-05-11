using event_booking_system.Common.DTOs.Categories;
using event_booking_system.Common.Entites;
using event_booking_system.Common.Utils;
using event_booking_system.Repositories.Interfaces;
using event_booking_system.Services.Interfaces;
using System.Collections.Generic;

namespace event_booking_system.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IEventRepository _eventRepository;

        public CategoryService(ICategoryRepository categoryRepository, IEventRepository eventRepository)
        {
            _categoryRepository = categoryRepository;
            _eventRepository = eventRepository;
        }

        public async Task<CategoryDTO> GetByIdAsync(int id)
        {
            var model = await _categoryRepository.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException($"Category with ID {id} was not found.");

            return MapToDTO(model);
        }

        public async Task<(List<CategoryDTO>,int)> GetAllAsync(int pageNumber, int pageSize = 12)
        {
            var model = await _categoryRepository.GetAllAsync(pageNumber, pageSize);
            if (model.Item2 == 0) return (new List<CategoryDTO>(), 0);
            var result = new List<CategoryDTO>();

            foreach (var item in model.Item1)
            {
                result.Add(MapToDTO(item));
            }
            return (result, model.Item2);
        }

        public CategoryDTO MapToDTO(Category item)
        {
            return new CategoryDTO
            {
                Id = item.Id,
                Name = item.Name,
                Event_List = item.Events?.Select(e => e.Title).ToList() ?? new List<string>()
            };
        }

        public async Task<Category> GetByNameAsync(string name, bool withEvents = false)
        {
            var category = await _categoryRepository.GetByNameAsync(name, withEvents);
            if (category == null)
                throw new NotFoundException($"Category with name '{name}' was not found.");
            return category;
        }

        public async Task<CategoryDTO> CreateCategoryAsync(string name)
        {
            var existing = await _categoryRepository.GetByNameAsync(name, false);
            if (existing != null)
                throw new ConflictException($"A category with the name '{name}' already exists.");

            var newCategory = new Category { Name = name };
            await _categoryRepository.AddAsync(newCategory);
            return MapToDTO(newCategory);
        }

        public async Task AddToCategoryAsync(int eventId, string name)
        {
            var category = await GetByNameAsync(name);
            var evt = await _eventRepository.GetByIdAsync(eventId);

            if (evt == null)
                throw new NotFoundException($"Event with ID {eventId} was not found.");

            evt.CategoryId = category.Id;
            await _eventRepository.UpdateAsync(evt);
        }

        public async Task RemoveFromCategoryAsync(int eventId, string name)
        {
            var category = await GetByNameAsync(name);
            var evt = await _eventRepository.GetByIdAsync(eventId);

            if (evt == null)
                throw new NotFoundException($"Event with ID {eventId} was not found.");

            evt.CategoryId = null;
            await _eventRepository.UpdateAsync(evt);
        }

        public async Task DeleteCategoryAsync(string name)
        {
            var category = await GetByNameAsync(name);
            if (category == null)
            {
                throw new Exception($"Category with name '{name}' not found.");
            }

            var containedEvents = await _eventRepository.GetEventsByCategory(category.Id);

            var dummy = await _categoryRepository.GetByNameAsync("None");
            if (dummy == null)
            {
                dummy = new Category() { Name = "None" };
                await _categoryRepository.AddAsync(dummy);
            }

            foreach (var item in containedEvents)
            {
                item.CategoryId = dummy.Id;
            }

            await _eventRepository.SaveAsync();

            await _categoryRepository.DeleteAsync(category);
        }


        public async Task<CategoryDTO> UpdateCategoryNameAsync(string newName, string oldName)
        {
            using (var transaction = await _categoryRepository.BeginTransactionAsync())
            {
                try
                {
                    var category = await GetByNameAsync(oldName);

                    var duplicate = await _categoryRepository.GetByNameAsync(newName, false);
                    if (duplicate != null)
                        throw new ConflictException($"Another category with the name '{newName}' already exists.");

                    category.Name = newName;
                    await _categoryRepository.UpdateAsync(category);

                    await transaction.CommitAsync();

                    return MapToDTO(category);
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }
}
