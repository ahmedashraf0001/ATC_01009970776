using event_booking_system.Common.Context;
using event_booking_system.Common.DTOs;
using event_booking_system.Common.Entites;
using event_booking_system.Common.QueryOptions.IncludeQueries;
using event_booking_system.Common.QueryOptions.SearchQueries;
using event_booking_system.Repositories.Interfaces;
using event_booking_system.Services.Generic.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Net.WebSockets;

namespace event_booking_system.Repositories.Implementations
{
    public class BookingRepository : Repository<Booking>, IBookingRepository
    {
        private readonly AppContextDb _context;

        public BookingRepository(AppContextDb context) : base(context)
        {
            _context = context;
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
        public async Task<int> GetBookingCount(int eventId)
        {
            return await _context.Bookings.Where(e=>e.EventId == eventId && e.Status != BookingStatus.Canceled).CountAsync();
        }
        public async Task<List<Booking>> GetBookingsByEventIdAsync(int eventId, BookingQuery options, int pageNumber, int pageSize = 12)
        {
            options ??= new BookingQuery();

            var query = _context.Bookings.Where(p => p.EventId == eventId);

            query = ApplyIncludes(query, options);

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task<(List<Booking>, int)> GetAllAsync(int pageNumber, int pageSize = 12, BookingQuery options = null)
        {
            options ??= new BookingQuery();

            var query = _context.Bookings.AsQueryable();

            query = ApplyIncludes(query, options);

            var bookings = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            var totalCount = await query.CountAsync();
            return (bookings, totalCount);
        }

        public async Task<decimal> GetRevenue()
        {
            return await _context.Bookings
                .Where(e => e.Status != BookingStatus.Canceled)
                .Join(
                    _context.Events,
                    b => b.EventId,
                    e => e.Id,
                    (b, e) => new { b.AdmissionTicketQty, b.VipTicketQty, e.AdmissionPrice, e.VipPrice }
                )
                .SumAsync(x =>
                    (x.AdmissionTicketQty * x.AdmissionPrice) +
                    (x.VipTicketQty * x.VipPrice));
        }

        public async Task<Booking> GetByIdAsync(int id, BookingQuery options)
        {
            options ??= new BookingQuery();
            var query = _context.Bookings.Where(p => p.Id == id);


            query = ApplyIncludes(query, options);

            return await query.FirstOrDefaultAsync();
        }
        public async Task<bool> IsBooked(string userId, int eventId) 
        {
            return await _context.Bookings.AnyAsync(e => e.UserId.Equals(userId) && e.EventId.Equals(eventId) && e.Status != BookingStatus.Canceled);
        }
        public async Task<Booking> GetByUserAndEventAsync(string userId, int eventId, BookingQuery options)
        {
            options ??= new BookingQuery();
           var query = _context.Bookings.Where(p => p.UserId == userId && p.EventId == eventId);


            query = ApplyIncludes(query, options);

            return await query.FirstOrDefaultAsync();
        }
        public async Task<List<Booking>> GetByUserIdAsync(string userId, BookingQuery options, int pageNumber, int pageSize = 12)
        {
            options ??= new BookingQuery();

            var query = _context.Bookings.Where(p => p.UserId == userId);
            query = ApplyIncludes(query, options);

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetCount()
        {
            return await _context.Bookings.CountAsync();
        }

        public async Task<List<Booking>> SearchBookingsAsync(BookingSearchQuery searchQuery, int pageNumber, int pageSize = 12)
        {
            var query = _context.Bookings.AsQueryable();

            query = ApplyIncludes(query, new BookingQuery
            {
                WithEvent = true,
                WithUser = true
            });

            if (!string.IsNullOrWhiteSpace(searchQuery.keyword))
            {
                var keyword = searchQuery.keyword.ToLower();
                query = query.Where(e =>
                     EF.Functions.Like(e.Event.Title, $"%{keyword}%") ||
                     EF.Functions.Like(e.User.UserName, $"%{keyword}%") ||
                     EF.Functions.Like(e.Event.Location, $"%{keyword}%"));
            }

            if (searchQuery.Status != null)
            {
                query = query.Where(e => e.Status == searchQuery.Status);
            }

            if (searchQuery.BookingDate != default)
            {
                var dateOnly = searchQuery.BookingDate.Date;
                var start = dateOnly;
                var end = start.AddDays(1);
                query = query.Where(e => e.BookingDate >= start && e.BookingDate < end);
            }

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        private IQueryable<Booking> ApplyIncludes(IQueryable<Booking> query, BookingQuery options)
        {
            if (options.WithEvent)
            {
                query = query.Include(b => b.Event);
            }

            if (options.WithUser)
            {
                query = query.Include(b => b.User);
            }

            return query;
        }
    }
}
