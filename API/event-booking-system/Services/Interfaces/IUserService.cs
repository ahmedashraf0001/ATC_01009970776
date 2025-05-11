using event_booking_system.Common.DTOs.Users;
using event_booking_system.Common.Entites;
using event_booking_system.Common.QueryOptions.SearchQueries;

namespace event_booking_system.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserProfileDTO> GetUserProfById(string UserId);
        Task<UserProfileDTO> GetUserProfByUsername(string username);
        Task<(List<UserProfileDTO>, int)> ListAllUsers(int pageNumber, int pageSize = 12);
        Task<bool> DeleteUser(string UserId);
        Task<UserProfileDTO> EditUser(UserEditDTO user);
        Task<List<UserDTO>> SearchUsers(UserSearchQuery searchQuery, int pageNumber, int pageSize = 12);
        Task<int> GetUsersCount();
    }
}
