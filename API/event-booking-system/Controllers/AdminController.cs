using event_booking_system.Common.DTOs.Others;
using event_booking_system.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace event_booking_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }
        [HttpGet("Dashboard")]
        public async Task<ActionResult<DashboardDTO>> dashboard()
        {
            var model = await _adminService.dashboard();
            return Ok(model);   
        }
    }
}
