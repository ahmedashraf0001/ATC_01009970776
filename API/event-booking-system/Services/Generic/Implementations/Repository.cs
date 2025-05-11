using event_booking_system.Common.Context;
using event_booking_system.Common.QueryOptions;
using event_booking_system.Services.Generic.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace event_booking_system.Services.Generic.Implementations
{
    public class Repository<T>: IRepository<T> where T : class
    {
        private readonly AppContextDb _context;
        public readonly DbSet<T> _set;
        public Repository(AppContextDb context)
        {
            _context = context;
            _set = context.Set<T>();
        }
        public virtual async Task AddAsync(T entity)
        {
            await _set.AddAsync(entity);
            await SaveAsync();
        }

        public virtual async Task DeleteAsync(T entity)
        {
            _set.Remove(entity);
            await SaveAsync();
        }

        public virtual async Task<List<T>> GetAllAsync()
        {
            return await _set.ToListAsync();
        }

        public virtual async Task<T?> GetByIdAsync(object id)
        {
            return await _set.FindAsync(id);
        }

        public virtual async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(T entity)
        {
            _context.Update(entity);
            await SaveAsync();
        }
    }
}
