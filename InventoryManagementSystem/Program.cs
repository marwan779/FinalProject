using InventoryManagementSystem.DataAccess.Data;
using InventoryManagementSystem.DataAccess.Repository;
using InventoryManagementSystem.DataAccess.Repository.IRepository;
using InventoryManagementSystem.Models.Entities;
using InventoryManagementSystem.Services.EmailService;
using InventoryManagementSystem.Services.ImageService;
using InventoryManagementSystem.Services.PaymentService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);




            builder.Services.AddDbContext<ApplicationDbContext>(options =>
     options.UseSqlServer(
         builder.Configuration.GetConnectionString("DefaultConnection"),
         b => b.MigrationsAssembly("InventoryManagementSystem")));



            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


            builder.Services.AddRazorPages();




            builder.Services.AddSingleton<IEmailSender, DummyEmailSender>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IImageService, ImageService>();

            //builder.Services.AddScoped<IProductRepository, ProductRepository>();
            //builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            //builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();






            // Add services to the container.
            builder.Services.AddControllersWithViews();



            // Payment Services
            builder.Services.AddHttpClient<IPaymentService, InstapayPaymentService>();
            builder.Services.AddScoped<IPaymentService, InstapayPaymentService>();





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

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
               name: "areas",
               pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();


            app.Run();
        }
    }
}
