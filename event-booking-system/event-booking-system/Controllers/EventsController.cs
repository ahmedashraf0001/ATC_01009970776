using event_booking_system.Common.DTOs.Events;
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
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;
        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }
        [HttpPost("Create")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<EventDTO>> Create([FromForm] CreateEventDTO request)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                throw new ValidationException(errors);
            }
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = await _eventService.CreateEventAsync(request, currentUserId);
            return Ok(model);
        }
        [HttpDelete("Delete/{eventId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int eventId)
        {
            await _eventService.DeleteEventAsync(eventId);
            return Ok("Deleted Sucessfully!");
        }
        [HttpPut("Edit/{eventId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<EventDTO>> Edit(int eventId,[FromForm] UpdateEventDTO request)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                throw new ValidationException(errors);
            }
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = await _eventService.UpdateEventAsync(eventId, request, currentUserId);
            return Ok(model);   
        }
        [HttpGet("Id/{id:int}")]

        public async Task<ActionResult<EventDetailsDTO>> Details(int id)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = await _eventService.GetEventDetailsAsync(id, currentUserId);
            return Ok(model);
        }
        [HttpGet("List/{pageNumber:int}/{pageSize:int}")]
        public async Task<ActionResult<EventListResponse>> List(int pageNumber, int pageSize = 12)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = await _eventService.GetEventsPageAsync(currentUserId, currentUserId, pageNumber, pageSize);
            var result = new EventListResponse
            {
                Events = model.Item1,
                TotalCount = model.Item2
            };
            return Ok(result);
        }
        [HttpGet("Search")]
        public async Task<ActionResult<EventListResponse>> Search(
            [FromQuery] EventSearchQuery searchQuery,
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
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = await _eventService.SearchEventsAsync(currentUserId,searchQuery, currentUserId, pageNumber, pageSize);
            var result = new EventListResponse
            {
                Events = model.Item1,
                TotalCount = model.Item2
            };
            return Ok(result);
        }
    }
}
