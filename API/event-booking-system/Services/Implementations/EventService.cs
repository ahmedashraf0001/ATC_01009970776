using event_booking_system.Common.DTOs.Events;
using event_booking_system.Common.Entites;
using event_booking_system.Common.QueryOptions.IncludeQueries;
using event_booking_system.Common.QueryOptions.SearchQueries;
using event_booking_system.Common.Utils;
using event_booking_system.Repositories.Implementations;
using event_booking_system.Repositories.Interfaces;
using event_booking_system.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

public class EventService : IEventService
{
    private readonly IEventRepository _eventRepo;
    private readonly ICategoryRepository _categoryRepo;
    private readonly IBookingService _bookingService;
    private readonly FileUpload _uploader;

    public EventService(IEventRepository eventRepo,  ICategoryRepository categoryRepo, FileUpload uploader, IBookingService bookingService)
    {
        _eventRepo = eventRepo;
        _categoryRepo = categoryRepo;
        _uploader = uploader;
        _bookingService = bookingService;
    }
    public async Task<int> GetTotalEventsCount()
    {
        return await _eventRepo.GetCount();   
    }
    public async Task<List<EventDashboardDTO>> recentEvents(int windowSize)
    {
        var model = await _eventRepo.recentEvents(windowSize);
        if (model == null)
            throw new NotFoundException($"Events not found.");

        List<EventDashboardDTO> list = new List<EventDashboardDTO>();
        foreach (var item in model)
        {
            list.Add(
                new EventDashboardDTO 
                {
                    EvTitle = item.Title,
                    EvDate = item.Date,
                    EvCategory = item.Category.Name,
                    numOfBooking = await _bookingService.GetBookingCount(item.Id),
                }
            );
        }
        return list;
    }
    public async Task<EventDTO> CreateEventAsync(CreateEventDTO request, string adminId)
    {
        var category = await _categoryRepo.GetByNameAsync(request.Category);
        if (category == null)
        {
            category = new Category() { Name = "None" };
            await _categoryRepo.AddAsync(category);
        }

        Event model = new Event
        {
            Title = request.Title,
            Description = request.Description,
            Date = request.Date,
            Location = request.Location,
            CategoryId = category.Id,
            AdmissionPrice = request.AdmissionPrice,
            VipPrice = request.VipPrice,
            AdmissionTicketQty = request.AdmissionTicketQty,
            VipTicketQty = request.VipTicketQty,
            CreatedById = adminId,
        };

        if (request.file != null)
        {
            var url = await _uploader.UploadAsync(request.file);
            model.ImageUrl = url;
        }
        await _eventRepo.AddAsync(model);

        var temp = await MapToEventDTO(model, category, adminId);

        return temp;
    }
    public async Task<bool> DeleteEventAsync(int eventId)
    {
        var model = await _eventRepo.GetByIdAsync(eventId);
        if (model == null)
            throw new NotFoundException($"Event with ID {eventId} not found.");

        await _uploader.DeleteAsync(model.ImageUrl);
        await _eventRepo.DeleteAsync(model);
        
        return true;
    }

    public async Task<EventDetailsDTO> GetEventDetailsAsync(int eventId, string userId)
    {
        var model = await _eventRepo.GetByIdAsync(eventId, new EventQuery { WithCategory = true, WithUserCreated = true });
        if (model == null)
            throw new NotFoundException($"Event with ID {eventId} not found.");

        EventDetailsDTO dto = new EventDetailsDTO
        {
            Id = model.Id,
            Title = model.Title,
            Description = model.Description,
            Category = model.Category?.Name ?? "Unknown",
            Date = model.Date,
            Location = model.Location,
            ImageUrl = model.ImageUrl,
            CreatedBy = model.CreatedBy?.UserName ?? "Unknown",
            IsBooked = await _bookingService.IsBooked(userId, eventId),
            AdmissionPrice = model.AdmissionPrice,
            VipPrice = model.VipPrice,
            AdmissionTicketQty = model.AdmissionTicketQty,
            VipTicketQty = model.VipTicketQty,
        };
        
        return dto;
    }

    public async Task<(List<EventDTO>, int)> GetEventsPageAsync(string userId, string CurrentUserId, int pageNumber, int pageSize = 12)
    {
        var model = await _eventRepo.GetListAsync(pageNumber, pageSize);
        if (model.Item2 == 0) return (new List<EventDTO>(), 0);

        var result = new List<EventDTO>();
        foreach (var item in model.Item1)
        {
            var category = await _categoryRepo.GetByIdAsync(item.CategoryId);
            if (category == null)
                throw new NotFoundException($"Category '{item.Category}' not found.");

            var temp = await MapToEventDTO(item, category, CurrentUserId);
            result.Add(temp);
        }
        return (result, model.Item2);
    }
    public async Task<(List<EventDTO>, int)> SearchEventsAsync(string userId, EventSearchQuery searchQuery, string CurrentUserId, int pageNumber, int pageSize = 12)
    {
        var model = await _eventRepo.SearchEventsAsync(searchQuery, pageNumber, pageSize);
        if (model.Item2 == 0) return (new List<EventDTO>(), 0);

        var result = new List<EventDTO>();
        foreach (var item in model.Item1)
        {
            var category = await _categoryRepo.GetByIdAsync(item.CategoryId);
            if (category == null)
                throw new NotFoundException($"Category '{item.Category}' not found.");
            var temp = await MapToEventDTO(item, category, CurrentUserId);
            result.Add(temp);
        }
        return (result, model.Item2);
    }
    public async Task<EventDTO> UpdateEventAsync(int eventId, UpdateEventDTO request, string CurrentUserId)
    {
        using (var transaction = await _eventRepo.BeginTransactionAsync())
        {
            try
            {
                var existing = await _eventRepo.GetByIdAsync(eventId);
                if (existing == null)
                    throw new NotFoundException($"Event with ID {eventId} not found.");

                var category = await _categoryRepo.GetByNameAsync(request.Category);
                if (category == null)
                    throw new NotFoundException($"Category '{request.Category}' not found.");

                await UpdateEventDetails(existing, request);
                await UpdateEventCategory(existing, category);
                if (request.file != null)
                    await UpdateEventImage(existing, request.file);

                await _eventRepo.UpdateAsync(existing);
                await transaction.CommitAsync();

                return await MapToEventDTO(existing, category, CurrentUserId);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

    private async Task UpdateEventDetails(Event existing, UpdateEventDTO request)
    {
        if (!string.IsNullOrWhiteSpace(request.Title))
            existing.Title = request.Title;

        if (!string.IsNullOrWhiteSpace(request.Description))
            existing.Description = request.Description;

        if (request.Date != default)
            existing.Date = request.Date.Value;

        if (!string.IsNullOrWhiteSpace(request.Location))
            existing.Location = request.Location;

        if (request.AdmissionPrice.HasValue)
            existing.AdmissionPrice = request.AdmissionPrice.Value;

        if (request.VipPrice.HasValue)
            existing.VipPrice = request.VipPrice.Value;

        if (request.AdmissionTicketQty.HasValue)
            existing.AdmissionTicketQty = request.AdmissionTicketQty.Value;

        if (request.VipTicketQty.HasValue)
            existing.VipTicketQty = request.VipTicketQty.Value;
    }

    private async Task UpdateEventCategory(Event existing, Category category)
    {
        existing.CategoryId = category.Id;
    }

    private async Task UpdateEventImage(Event existing, IFormFile file)
    {
        await _uploader.DeleteAsync(existing.ImageUrl);
        var url = await _uploader.UploadAsync(file);
        existing.ImageUrl = url;
    }

    private async Task<EventDTO> MapToEventDTO(Event existing, Category category, string CurrentUserId)
    {
        return new EventDTO
        {
            Id = existing.Id,
            Title = existing.Title,
            Category = category.Name,
            Date = existing.Date,
            Location = existing.Location,
            Price = existing.AdmissionPrice,
            ImageUrl = existing.ImageUrl,
            AdmissionPrice = existing.AdmissionPrice,
            VipPrice = existing.VipPrice,
            AdmissionTicketQty = existing.AdmissionTicketQty,
            VipTicketQty = existing.VipTicketQty,
            Description = existing.Description,
            SoldOut = existing.AdmissionTicketQty == 0 && existing.VipTicketQty == 0,
            IsBooked = await _bookingService.IsBooked(CurrentUserId, existing.Id)
        };
    }


}
