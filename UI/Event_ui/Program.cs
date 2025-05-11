using Event_ui.Controllers;
using Event_ui.Util;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Event_ui
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddHttpClient<EventController>();
            builder.Services.AddHttpClient<AdminController>();
            builder.Services.AddHttpClient<UserController>();
            builder.Services.AddHttpClient<AccountController>();

            builder.Services.AddScoped<ErrorsHandler>();
            builder.Services.AddControllersWithViews();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                            .AddCookie(options =>
                            {
                                options.Cookie.HttpOnly = true;
                                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                                options.Cookie.SameSite = SameSiteMode.Strict;
                                options.LoginPath = "/Account/Login";
                                options.LogoutPath = "/Account/Logout";
                                options.AccessDeniedPath = "/Account/AccessDenied";
                                options.ExpireTimeSpan = TimeSpan.FromDays(1);
                                options.SlidingExpiration = true;
                            });

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Event}/{action=Index}/{pageNumber=1}/{pageSize=12}");

            app.Run();
        }
    }
}
