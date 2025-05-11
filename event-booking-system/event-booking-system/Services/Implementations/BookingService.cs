using event_booking_system.Common.DTOs.Bookings;
using event_booking_system.Common.Entites;
using event_booking_system.Common.QueryOptions.IncludeQueries;
using event_booking_system.Common.QueryOptions.SearchQueries;
using event_booking_system.Common.Utils;
using event_booking_system.Repositories.Interfaces;
using event_booking_system.Services.Interfaces;


public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly ICategoryService _categoryService;
    private readonly IEventRepository _eventRepo;

    public BookingService(IEventRepository eventRepo, IBookingRepository bookingRepository, ICategoryService categoryService)
    {
        _bookingRepository = bookingRepository;
        _categoryService = categoryService;
        _eventRepo = eventRepo;
    }
    public async Task<int> GetBookingCount(int eventId) 
    {
        return await _bookingRepository.GetBookingCount(eventId);
    }

    public async Task<BookingDTO> BookEventAsync(CreateBookingDTO request, string userId)
    {
        using (var transaction = await _bookingRepository.BeginTransactionAsync())
        {
            try
            {
                var ev = await _eventRepo.GetByIdAsync(request.EventId);
                if (ev == null)
                    throw new ValidationException("Event does not exist.");

                var model = new Booking()
                {
                    UserId = userId,
                    EventId = request.EventId,
                    TicketType = request.TicketType,
                    BookingDate = DateTime.Now,
                    Status = BookingStatus.Pending,
                };

                await ProcessTicketQuantitiesAsync(ev, request, model);
                await _bookingRepository.AddAsync(model);

                model = await _bookingRepository.GetByIdAsync(model.Id, new BookingQuery { WithEvent = true, WithUser = true });

                if (model.Event == null || model.Event.CategoryId == null)
                    throw new ValidationException("Event or its category is not assigned.");

                var category = await _categoryService.GetByIdAsync(model.Event.CategoryId.Value);

                await transaction.CommitAsync();

                return await MapToDTO(model, category.Name);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
    private async Task ProcessTicketQuantitiesAsync(Event ev, CreateBookingDTO request, Booking model)
    {
        if (request.AdmissionQty.HasValue)
        {
            model.AdmissionTicketQty = request.AdmissionQty.Value;
            if (ev.AdmissionTicketQty - request.AdmissionQty.Value < 0)
            {
                throw new ValidationException("The quantity for the admission tickets provided exceeds the current available.");
            }
            ev.AdmissionTicketQty -= request.AdmissionQty.Value;
        }

        if (request.VipQty.HasValue)
        {
            model.VipTicketQty = request.VipQty.Value;
            if (ev.VipTicketQty - request.VipQty.Value < 0)
            {
                throw new ValidationException("The quantity for the VIP tickets provided exceeds the current available.");
            }
            ev.VipTicketQty -= request.VipQty.Value;
        }
    }


    public async Task<List<BookingDTO>> GetBookingsByEventIdAsync(int eventId, BookingQuery options, int pageNumber, int pageSize = 12)
    {
        var bookings = await _bookingRepository.GetBookingsByEventIdAsync(eventId, new BookingQuery { WithUser = true, WithEvent = true }, pageNumber, pageSize);
        
        var result = new List<BookingDTO>();
        foreach (var booking in bookings)
        {
            if (booking.Event?.CategoryId == null)
                throw new ValidationException("Event category is not assigned.");

            var category = await _categoryService.GetByIdAsync(booking.Event.CategoryId.Value);
            result.Add(await MapToDTO(booking, category.Name));
        }
        return result;

    }
    public async Task<List<BookingDTO>> GetBookingsByUserIdAsync(string userId, int pageNumber, int pageSize = 12)
    {
        var bookings = await _bookingRepository.GetByUserIdAsync(userId, new BookingQuery { WithUser = true, WithEvent = true}, pageNumber, pageSize);
        var result = new List<BookingDTO>();

        foreach (var booking in bookings)
        {
            if (booking.Event?.CategoryId == null)
                throw new ValidationException("Event category is not assigned.");

            var category = await _categoryService.GetByIdAsync(booking.Event.CategoryId.Value);
            result.Add(await MapToDTO(booking, category.Name));
        }

        return result;
    }

    public async Task<BookingDTO> ChangeStatus(BookingStatus status, int bookingId)
    {
        var model = await _bookingRepository.GetByIdAsync(bookingId, new BookingQuery { WithEvent = true, WithUser = true });

        if (model == null)
            throw new NotFoundException($"Booking with ID {bookingId} not found.");

        if (model.Event?.CategoryId == null)
            throw new ValidationException("Event category is not assigned.");

        model.Status = status;
        await _bookingRepository.UpdateAsync(model);

        var category = await _categoryService.GetByIdAsync(model.Event.CategoryId.Value);
        return await MapToDTO(model, category.Name);
    }

    public async Task<BookingDTO> GetByIdAsync(int id)
    {
        var model = await _bookingRepository.GetByIdAsync(id, new BookingQuery { WithEvent = true, WithUser = true });

        if (model == null)
            throw new NotFoundException($"Booking with ID {id} not found.");

        if (model.Event?.CategoryId == null)
            throw new ValidationException("Event category is not assigned.");

        var category = await _categoryService.GetByIdAsync(model.Event.CategoryId.Value);
        return await MapToDTO(model, category.Name);
    }
    public async Task<BookingDTO> MapToDTO(Booking model, string categoryName)
    {
        decimal evPrice = (model.AdmissionTicketQty * model.Event.AdmissionPrice) +
                  (model.VipTicketQty * model.Event.VipPrice);
        return new BookingDTO
        {
            Id = model.Id,
            EvId = model.EventId,
            EvTitle = model.Event.Title,
            EvImageUrl = model.Event.ImageUrl,
            EvCategory = categoryName,
            EvLocation = model.Event.Location,
            EvDate = model.Event.Date,
            userName = model.User.UserName,
            TicketType = model.TicketType,
            BookingDate = model.BookingDate,
            Status = model.Status,
            EvPrice = evPrice,
            AdmissionTicketQty = model.AdmissionTicketQty,
            VipTicketQty = model.VipTicketQty,
            AdmissionPrice = model.Event.AdmissionPrice,
            VipPrice = model.Event.VipPrice
        };
    }

    public async Task<(List<BookingDTO>, int)> ListAllBookings(int pageNumber, int pageSize = 12)
    {
        var model = await _bookingRepository.GetAllAsync(pageNumber, pageSize, new BookingQuery { WithEvent = true, WithUser = true});

        if (model.Item2 == 0) return (new List<BookingDTO>(), 0);

        var result = new List<BookingDTO>();

        foreach (var booking in model.Item1)
        {
            if (booking.Event?.CategoryId == null)
                throw new ValidationException("Event category is not assigned.");

            var category = await _categoryService.GetByIdAsync(booking.Event.CategoryId.Value);
            result.Add(await MapToDTO(booking, category.Name));
        }

        return (result, model.Item2);
    }

    public async Task<BookingDTO> UnBookingAsync(int id)
    {
        using (var transaction = await _bookingRepository.BeginTransactionAsync())
        {
            try
            {
                var model = await _bookingRepository.GetByIdAsync(id, new BookingQuery { WithEvent = true, WithUser = true });
                if (model == null)
                    throw new NotFoundException($"Booking with ID {id} not found.");

                if (model.Event?.CategoryId == null)
                    throw new ValidationException("Event category is not assigned.");

                var category = await _categoryService.GetByIdAsync(model.Event.CategoryId.Value);

                model.Status = BookingStatus.Canceled;
                model.Event.AdmissionTicketQty += model.AdmissionTicketQty;
                model.Event.VipTicketQty += model.VipTicketQty;

                await _bookingRepository.UpdateAsync(model);

                await transaction.CommitAsync();

                return await MapToDTO(model, category.Name);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

    public async Task<List<BookingDTO>> SearchBookingsAsync(BookingSearchQuery searchQuery, int pageNumber, int pageSize = 12)
    {
        var model = await _bookingRepository.SearchBookingsAsync(searchQuery, pageNumber, pageSize);
        if (!model.Any()) return new List<BookingDTO>();

        var result = new List<BookingDTO>();
        foreach (var booking in model)
        {
            if (booking.Event?.CategoryId == null)
                throw new ValidationException("Event category is not assigned.");

            var category = await _categoryService.GetByIdAsync(booking.Event.CategoryId.Value);
            result.Add(await MapToDTO(booking, category.Name));
        }

        return result;
    }
    public async Task<decimal> GetRevenue()
    {
        return await _bookingRepository.GetRevenue();
    }
    public async Task<int> GetBookingCount()
    {
        return await _bookingRepository.GetCount();
    }

    public async Task<bool> IsBooked(string userId, int eventId)
    {
        return await _bookingRepository.IsBooked(userId, eventId);
    }
}
