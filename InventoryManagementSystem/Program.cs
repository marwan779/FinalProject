using InventoryManagementSystem.DataAccess.Data;
using InventoryManagementSystem.DataAccess.Repository;
using InventoryManagementSystem.DataAccess.Repository.IRepository;
using InventoryManagementSystem.Models.Entities;
using InventoryManagementSystem.Services.EmailService;
using InventoryManagementSystem.Services.ImageService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 🔹 Database connection
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // 🔹 Identity setup
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // 🔹 Dependency Injection
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IImageService, ImageService>();
            builder.Services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
            builder.Services.AddSingleton<IEmailSender, DummyEmailSender>();

            // 🔹 MVC + Razor Pages
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // 🔹 Exception handling
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // 🔹 Middlewares
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            // 🔹 Routing
            app.MapControllerRoute(
               name: "areas",
               pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();

            // 🔹 Create Roles + Default Owner Account
            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                // ✅ Only the roles you want
                string[] roleNames = { "Owner", "Supplier", "Customer" };

                foreach (var roleName in roleNames)
                {
                    if (!await roleManager.RoleExistsAsync(roleName))
                    {
                        await roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }

                // ✅ Default Owner user
                string ownerEmail = "owner@store.com";
                string ownerPassword = "Owner@123";

                var ownerUser = await userManager.FindByEmailAsync(ownerEmail);
                if (ownerUser == null)
                {
                    var newOwner = new ApplicationUser
                    {
                        UserName = ownerEmail,
                        Email = ownerEmail,
                        EmailConfirmed = true,
                        FullName = "System Owner",
                        Address = "System Address"
                    };

                    var createUserResult = await userManager.CreateAsync(newOwner, ownerPassword);
                    if (createUserResult.Succeeded)
                    {
                        await userManager.AddToRoleAsync(newOwner, "Owner");
                    }
                }
            }

            app.Run();
        }
    }
}
