using event_booking_system.Common.DTOs.Others;

namespace event_booking_system.Services.Interfaces
{
    public interface IAdminService
    {
        Task<DashboardDTO> dashboard();
    }
}
