using event_booking_system.Common.Entites;
using event_booking_system.Common.QueryOptions.IncludeQueries;
using event_booking_system.Common.QueryOptions.SearchQueries;
using event_booking_system.Services.Generic.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace event_booking_system.Repositories.Interfaces
{
    public interface IUserRepository: IRepository<User>
    {
        Task<User?> GetByIdAsync(object id, UserQuery options = null);
        Task<User> GetByUsernameAsync(string username, UserQuery options = null);
        Task<User> GetByEmailAsync(string email, UserQuery options = null);
        Task<List<User>> SearchUsersAsync(UserSearchQuery searchQuery, int pageNumber, int pageSize = 12);
        Task DeleteUserAsync(string userId);
        Task<(List<User>, int)> GetAllAsync(int pageNumber, int pageSize = 12);
        Task<int> GetCount();
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
