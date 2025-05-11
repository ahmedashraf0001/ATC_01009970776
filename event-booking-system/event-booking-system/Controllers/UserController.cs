using event_booking_system.Common.DTOs.Users;
using event_booking_system.Common.Entites;
using event_booking_system.Common.QueryOptions.SearchQueries;
using event_booking_system.Common.Utils;
using event_booking_system.Services.Implementations;
using event_booking_system.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace event_booking_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet("Details")]
        public async Task<ActionResult<UserProfileDTO>> Details()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = await _userService.GetUserProfById(currentUserId);
            return Ok(model);
        }
        [HttpGet("Username/{username}")]
        public async Task<ActionResult<UserProfileDTO>> DetailsByUsername(string username)
        {
            var model = await _userService.GetUserProfByUsername(username);
            return Ok(model);
        }
        [HttpGet("List/{pageNumber:int}/{pageSize:int}")]
        public async Task<ActionResult<UserListResponse>> List(int pageNumber, int pageSize = 12)
        {
            var model = await _userService.ListAllUsers(pageNumber, pageSize);
            var result = new UserListResponse
            {
                Users = model.Item1,
                TotalCount = model.Item2
            };
            return Ok(result);
        }
        [HttpGet("Search")]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult<List<UserDTO>>> Search(
            [FromQuery] UserSearchQuery searchQuery,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 12
        )
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                throw new ValidationException(errors);
            }
            var model = await _userService.SearchUsers(searchQuery, pageNumber, pageSize);
            return Ok(model);
        }
        [HttpPut("Edit")]
        public async Task<ActionResult<UserProfileDTO>> Edit([FromForm] UserEditDTO user)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                throw new ValidationException(errors);
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUserRole = User.FindFirstValue(ClaimTypes.Role);

            var isAdmin = currentUserRole == "Admin";

            if (currentUserId != user.Id && !isAdmin)
                throw new UnauthorizedException("You can only edit your own profile.");

            if (user.Role.HasValue && !isAdmin)
                throw new UnauthorizedException("Only admins can change user roles.");

            var model = await _userService.EditUser(user);
            return Ok(model);
        }

        [HttpDelete("Delete/{userid}")]
        public async Task<ActionResult> Delete(string userid)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUserRole = User.FindFirstValue(ClaimTypes.Role);

            if (currentUserId != userid && currentUserRole != "Admin")
            {
                throw new UnauthorizedException("Only admins can change user roles.");
            }
            await _userService.DeleteUser(userid);
            return Ok("Deleted Sucessfully!");
        }
    }
}
