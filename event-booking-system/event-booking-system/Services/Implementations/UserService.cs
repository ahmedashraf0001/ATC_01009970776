using event_booking_system.Common.DTOs.Users;
using event_booking_system.Common.Entites;
using event_booking_system.Common.QueryOptions.SearchQueries;
using event_booking_system.Common.Utils;
using event_booking_system.Repositories.Interfaces;
using event_booking_system.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace event_booking_system.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly UserManager<User> _manager;
        private readonly FileUpload _uploader;
        public UserService(IUserRepository userRepository, UserManager<User> manager, FileUpload uploader)
        {
            _userRepo = userRepository;
            _manager = manager;
            _uploader = uploader;

        }
        public async Task<bool> DeleteUser(string userId)
        {
            var existingUser = await _userRepo.GetByIdAsync(userId);
            if (existingUser == null)
                throw new NotFoundException($"User with ID {userId} not found.");

            await _userRepo.DeleteUserAsync(userId);
            return true;
        }
        public async Task<int> GetUsersCount()
        {
            return await _userRepo.GetCount();
        }
        public async Task<UserProfileDTO> EditUser(UserEditDTO request)
        {
            using (var transaction = await _userRepo.BeginTransactionAsync()) 
            {
                try
                {
                    var model = await _userRepo.GetByIdAsync(request.Id);
                    if (model == null)
                        throw new NotFoundException($"User with ID {request.Id} not found.");

                    await EditUserDetails(model, request);

                    if (request.Role.HasValue)
                        await UpdateUserRole(model, request.Role.Value);

                    if (!string.IsNullOrWhiteSpace(request.NewPassword))
                        await ChangePassword(model, request);

                    if (request.file != null)
                        await UpdateUserImage(model, request.file);

                    await _userRepo.UpdateAsync(model);
                    await transaction.CommitAsync();

                    return new UserProfileDTO
                    {
                        Id = model.Id,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        Address = model.Address,
                        Bio = model.Bio,
                        PhoneNumber = model.PhoneNumber,
                        ImageUrl = model.ImageUrl,
                        Role = model.Role
                    };
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync(); 
                    throw;
                }
            }
        }

        private async Task EditUserDetails(User model, UserEditDTO request)
        {
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                var nameParts = Regex.Split(request.Name, @"[_\s]+", RegexOptions.Compiled | RegexOptions.CultureInvariant);
                model.FirstName = nameParts.FirstOrDefault();
                model.LastName = nameParts.Length > 1 ? string.Join(" ", nameParts.Skip(1)) : string.Empty;
            }

            if (!string.IsNullOrWhiteSpace(request.Email))
                model.Email = request.Email;

            if (!string.IsNullOrWhiteSpace(request.Address))
                model.Address = request.Address;

            if (!string.IsNullOrWhiteSpace(request.Phone))
                model.PhoneNumber = request.Phone;

            if (!string.IsNullOrWhiteSpace(request.Bio))
                model.Bio = request.Bio;
        }

        private async Task UpdateUserRole(User model, RoleType role)
        {
            var roles = await _manager.GetRolesAsync(model);
            if (roles.Any())
            {
                var removeRolesResult = await _manager.RemoveFromRolesAsync(model, roles);
                if (!removeRolesResult.Succeeded)
                    throw new ValidationException("Failed to remove existing roles.");
            }

            var addRoleResult = await _manager.AddToRoleAsync(model, role.ToString());
            if (!addRoleResult.Succeeded)
            {
                var errors = string.Join(", ", addRoleResult.Errors.Select(e => e.Description));
                throw new ValidationException($"Role update failed: {errors}");
            }

            model.Role = role;
        }

        private async Task ChangePassword(User model, UserEditDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.CurrentPassword))
                throw new ValidationException("Current password is required to change the password.");

            var isPasswordValid = await _manager.CheckPasswordAsync(model, request.CurrentPassword);
            if (!isPasswordValid)
                throw new ValidationException("Current password is incorrect or the same as the old one.");

            var removeResult = await _manager.RemovePasswordAsync(model);
            if (!removeResult.Succeeded)
                throw new ValidationException("Failed to remove existing password.");

            var addResult = await _manager.AddPasswordAsync(model, request.NewPassword);


            if (!addResult.Succeeded)
            {
                var errors = string.Join(", ", addResult.Errors.Select(e => e.Description));
                throw new ValidationException($"Password update failed: {errors}");
            }
        }

        private async Task UpdateUserImage(User model, IFormFile file)
        {
            await _uploader.DeleteAsync(model.ImageUrl);
            var url = await _uploader.UploadAsync(file);
            model.ImageUrl = url;
        }


        public async Task<UserProfileDTO> GetUserProfById(string userId)
        {
            var model = await _userRepo.GetByIdAsync(userId);
            if (model == null)
                throw new NotFoundException($"User with ID {userId} not found.");

            return new UserProfileDTO
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Address = model.Address,
                Bio = model.Bio,
                PhoneNumber = model.PhoneNumber,
                ImageUrl = model.ImageUrl,
                Role = model.Role
            };
        }
        public async Task<UserProfileDTO> GetUserProfByUsername(string username)
        {
            var model = await _userRepo.GetByUsernameAsync(username);
            if (model == null)
                throw new NotFoundException($"User with username '{username}' not found.");

            return new UserProfileDTO
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Address = model.Address,
                Bio = model.Bio,
                PhoneNumber = model.PhoneNumber,
                ImageUrl = model.ImageUrl,
                Role = model.Role
            };
        }
        public async Task<(List<UserProfileDTO>, int)> ListAllUsers(int pageNumber, int pageSize = 12)
        {
            var model = await _userRepo.GetAllAsync(pageNumber, pageSize);
            if (model.Item2 == 0) return (new List<UserProfileDTO>(), 0);

            List<UserProfileDTO> result = model.Item1.Select(user => new UserProfileDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Address = user.Address,
                Bio = user.Bio,
                PhoneNumber = user.PhoneNumber,
                ImageUrl = user.ImageUrl,
                Role = user.Role
            }).ToList();

            return (result, model.Item2);
        }

        public async Task<List<UserDTO>> SearchUsers(UserSearchQuery searchQuery, int pageNumber, int pageSize = 12)
        {
            var model = await _userRepo.SearchUsersAsync(searchQuery, pageNumber, pageSize);
            if (!model.Any()) return new List<UserDTO>();

            return model.Select(user => new UserDTO
            {
                Id = user.Id,
                Name = $"{user.FirstName} {user.LastName}".Trim(),
                Email = user.Email,
                Role = user.Role,
                DateJoined = user.DateJoined
            }).ToList();
        }

    }
}
