using event_booking_system.Repositories.Implementations;
using event_booking_system.Repositories.Interfaces;
using event_booking_system.Services.Implementations;
using event_booking_system.Services.Interfaces;

namespace event_booking_system.Common.Utils
{
    public static class ServiceCollection
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();


            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IAdminService, AdminService>();


            services.AddScoped<FileUpload>();

            return services;
        }
    }
    public static class AdminSeed
    {
        public static async Task SeedAdminUser(IServiceProvider serviceProvider)
        {
            var _authService = serviceProvider.GetRequiredService<IAuthService>();

            await _authService.InitAdminAccount();
        }
    }
}
