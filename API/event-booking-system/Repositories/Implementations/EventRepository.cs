using event_booking_system.Common.Context;
using event_booking_system.Common.Entites;
using event_booking_system.Common.QueryOptions.IncludeQueries;
using event_booking_system.Common.QueryOptions.SearchQueries;
using event_booking_system.Repositories.Interfaces;
using event_booking_system.Services.Generic.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace event_booking_system.Repositories.Implementations
{
    public class EventRepository : Repository<Event>, IEventRepository
    {
        private readonly AppContextDb _context;
        public EventRepository(AppContextDb context) : base(context)
        {
            _context = context;
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
        public async Task<int> GetCount()
        {
            return await _context.Events.CountAsync();
        }
        public async Task<List<Event>> recentEvents(int windowSize)
        {
            return await _context.Events.Include(e => e.Category).OrderByDescending(e => e.Date)
                         .Take(windowSize)
                         .ToListAsync();
        }
        public async Task<(List<Event>, int)> GetListAsync(int pageNumber, int pageSize = 12)
        {
            var query = _context.Events.AsQueryable();

            var events = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

            var totalCount = await query.CountAsync();
            return (events, totalCount);
        }
        public async Task<Event?> GetByIdAsync(int id, EventQuery options = null)
        {
            options ??= new EventQuery();
            var query = ApplyIncludes(_context.Events.Where(p => p.Id == id), options);

            return await query.FirstOrDefaultAsync();
        }
        public async Task<List<Event>> GetByUserIdAsync(string userId, EventQuery options)
        {
            options ??= new EventQuery();
            var query = ApplyIncludes(_context.Events.Where(p => p.CreatedById == userId), options);

            return await query.ToListAsync();
        }
        public async Task<(List<Event>, int)> SearchEventsAsync(EventSearchQuery searchQuery, int pageNumber, int pageSize = 12)
        {
            var query = _context.Events.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchQuery.Keyword))
            {
                var keyword = searchQuery.Keyword.ToLower();
                query = query
                    .Include(e => e.CreatedBy)
                    .Include(e => e.Category)
                    .Where(e =>
                        EF.Functions.Like(e.Title.ToLower(), $"%{keyword}%") ||
                        EF.Functions.Like(e.Description.ToLower(), $"%{keyword}%") ||
                        EF.Functions.Like(e.Location.ToLower(), $"%{keyword}%") ||
                        EF.Functions.Like(e.CreatedBy.UserName.ToLower(), $"%{keyword}%") ||
                        EF.Functions.Like(e.Category.Name.ToLower(), $"%{keyword}%"));
            }
            if (searchQuery.Price.HasValue)
            {
                decimal price = searchQuery.Price.Value;
                query = query.Where(e =>
                    Math.Abs(e.VipPrice - price) < 1m ||
                    Math.Abs(e.AdmissionPrice - price) < 1m);
            }

            if (searchQuery.Date != default)
            {
                var start = searchQuery.Date.Value.Date;
                var end = start.AddDays(1);
                query = query.Where(e => e.Date >= start && e.Date < end);
            }

            var events = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalCount = await query.CountAsync();
            return (events, totalCount);
        }
        private IQueryable<Event> ApplyIncludes(IQueryable<Event> query, EventQuery options)
        {
            if (options.WithUserCreated)
                query = query.Include(e => e.CreatedBy);
            if (options.WithCategory)
                query = query.Include(e => e.Category);
            if (options.WithBookings)
                query = query.Include(e => e.Bookings);
            return query;
        }
        public async Task<List<Event>> GetEventsByCategory(int CategoryId)
        {
            return await _context.Events.Where(e => e.CategoryId == CategoryId).ToListAsync();
        }
    }
}
