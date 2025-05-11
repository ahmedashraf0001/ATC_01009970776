using event_booking_system.Common.Context;
using event_booking_system.Common.DTOs;
using event_booking_system.Common.Entites;
using event_booking_system.Common.QueryOptions.IncludeQueries;
using event_booking_system.Common.QueryOptions.SearchQueries;
using event_booking_system.Repositories.Interfaces;
using event_booking_system.Services.Generic.Implementations;
using MailKit.Search;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;

namespace event_booking_system.Repositories.Implementations
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly AppContextDb _context;
        public UserRepository(AppContextDb context) : base(context)
        {
            _context = context;
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
        public async Task DeleteUserAsync(string userId)
        {
            var model = await _context.Users.FindAsync(userId);
            await DeleteAsync(model);
        }
        public async Task<int> GetCount()
        {
            return await _context.Users.CountAsync();
        }
        public async Task<User?> GetByIdAsync(object id, UserQuery options = null)
        {
            options ??= new UserQuery();
            var query = ApplyIncludes(_context.Users.Where(u => u.Id == id), options);

            return await query.FirstOrDefaultAsync();
        }
        public async Task<User> GetByEmailAsync(string email, UserQuery options = null)
        {
            options ??= new UserQuery();
            var query = ApplyIncludes(_context.Users.Where(u => u.Email == email), options);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<User> GetByUsernameAsync(string username, UserQuery options = null)
        {
            options ??= new UserQuery();
            var query = ApplyIncludes(_context.Users.Where(u => u.UserName == username), options);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<User>> SearchUsersAsync(UserSearchQuery searchQuery, int pageNumber, int pageSize = 12)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchQuery.Keyword))
            {
                var keyword = searchQuery.Keyword.ToLower();
                query = query
                    .Where(e =>
            EF.Functions.Like(e.FirstName, $"%{keyword}%") ||
            EF.Functions.Like(e.LastName, $"%{keyword}%") ||
            EF.Functions.Like(e.Address, $"%{keyword}%") ||
            EF.Functions.Like(e.Bio, $"%{keyword}%") ||
            e.PhoneNumber == keyword);

            }
            if (searchQuery.RoleType.HasValue)
            {
                query = query.Where(e => e.Role.Equals(searchQuery.RoleType));
            }

            if (searchQuery.DateJoined != default)
            {
                var dateOnly = searchQuery.DateJoined;
                var start = dateOnly.Date;
                var end = start.AddDays(1);
                query = query.Where(e => e.DateJoined >= start && e.DateJoined < end);
            }

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        private IQueryable<User> ApplyIncludes(IQueryable<User> query, UserQuery options)
        {
            if (options.WithBookings)
                query = query.Include(u => u.Bookings);
            if (options.WithCreatedEv)
                query = query.Include(u => u.CreatedEvents);
            return query;
        }

        public async Task<(List<User>, int)> GetAllAsync(int pageNumber, int pageSize = 12)
        {
            var query = _context.Users.AsQueryable();

            var users = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalCount = await query.CountAsync();
            return (users, totalCount);

        }
    }
}
