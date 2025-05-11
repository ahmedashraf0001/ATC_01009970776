using event_booking_system.Common.Context;
using event_booking_system.Common.Entites;
using event_booking_system.Repositories.Interfaces;
using event_booking_system.Services.Generic.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace event_booking_system.Repositories.Implementations
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository 
    {
        private readonly AppContextDb _context;
        public CategoryRepository(AppContextDb context) : base(context)
        {
            _context = context;
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
        public async Task<Category> GetByNameAsync(string name, bool withEvents = false)
        {
            var query = _context.Categories.Where(p => p.Name == name);
            query = ApplyIncludes(query, withEvents);

            return await query.FirstOrDefaultAsync();
        }
        public async Task<(List<Category>, int)> GetAllAsync(int pageNumber, int pageSize = 12)
        {
            var query = _context.Categories.AsQueryable();
            query = ApplyIncludes(query, true);
            var totalCount = await query.CountAsync();
            var categories = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (categories, totalCount);
        }
        public override async Task<Category?> GetByIdAsync(object id)
        {
            var query = _context.Categories.Where(e => e.Id == (int)id);
            query = ApplyIncludes(query, true);

            return await query.FirstOrDefaultAsync();
        }
        private IQueryable<Category> ApplyIncludes(IQueryable<Category> query, bool withEvents)
        {
            if (withEvents)
            {
                query = query.Include(u => u.Events);
            }
            return query;
        }
    }
}
