using Microsoft.EntityFrameworkCore;
using QuanLyBanHangDoAnThucUong.Data;

namespace QuanLyBanHangDoAnThucUong
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Add DbContext
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Add Session
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            // Add Route Lowercase
            builder.Services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Add Session middleware
            app.UseSession();

            app.UseAuthorization();

            // Custom SEO Route for MonAn Details: /mon-an/{slug}-{id}
            app.MapControllerRoute(
                name: "monAnSEO",
                pattern: "mon-an/{slug}-{id}",
                defaults: new { controller = "Home", action = "MonAnDetails" });

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // Custom SEO Route for Store Details: cua-hang/{cityslug}/{storeslug}-{id}
            app.MapControllerRoute(
                name: "storeSEO",
                pattern: "cua-hang/{cityslug}/{storeslug}-{id}",
                defaults: new { controller = "Home", action = "StoreDetails" });

            app.Run();
        }
    }
}