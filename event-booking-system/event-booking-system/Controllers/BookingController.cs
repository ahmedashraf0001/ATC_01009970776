using event_booking_system.Common.DTOs.Bookings;
using event_booking_system.Common.Entites;
using event_booking_system.Common.QueryOptions.SearchQueries;
using event_booking_system.Common.Utils;
using event_booking_system.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace event_booking_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        public BookingController(IBookingService bookingService) 
        {
            _bookingService = bookingService;
        }
        [HttpPost("Book")]
        public async Task<ActionResult<BookingDTO>> Book(CreateBookingDTO request)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                throw new ValidationException(errors);
            }
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = await _bookingService.BookEventAsync(request, currentUserId);
            return Ok(model);
        }
        [HttpDelete("Unbook/{BookingId:int}")]
        public async Task<ActionResult<BookingDTO>> Unbook(int BookingId)
        {
            var model = await _bookingService.UnBookingAsync(BookingId);
            return Ok(model);
        }
        [HttpPut("UpdateState/{BookingId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<BookingDTO>> UpdateState(int BookingId, BookingStatus status)
        {
            var model = await _bookingService.ChangeStatus(status, BookingId);
            return Ok(model);
        }
        [HttpGet("Id/{BookingId:int}")]

        public async Task<ActionResult<BookingDTO>> Details(int BookingId)
        {
            var model = await _bookingService.GetByIdAsync(BookingId);
            return Ok(model);
        }
        [HttpGet("ListAll/{pageNumber:int}/{pageSize:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<BookingDTO>>> List(int pageNumber, int pageSize = 12)
        {
            var model = await _bookingService.ListAllBookings(pageNumber, pageSize);
            var result = new BookingListResponse
            {
                Bookings = model.Item1,
                TotalCount = model.Item2
            };
            return Ok(result);
        }
        [HttpGet("List/Current/{pageNumber:int}/{pageSize:int}")]
        public async Task<ActionResult<List<BookingDTO>>> ListCurrent(int pageNumber, int pageSize = 12)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = await _bookingService.GetBookingsByUserIdAsync(currentUserId, pageNumber, pageSize);
            return Ok(model);
        }
        [HttpGet("Search/{pageNumber:int}/{pageSize:int}")]
        public async Task<ActionResult<List<BookingDTO>>> Search(
            [FromQuery] BookingSearchQuery searchQuery,
            int pageNumber = 1,
            int pageSize = 12
        )
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                throw new ValidationException(errors);
            }
            var model = await _bookingService.SearchBookingsAsync(searchQuery, pageNumber, pageSize);
            return Ok(model);
        }
    }
}
