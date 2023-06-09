﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyShopPet.AutoMapperProfile;
using MyShopPet.Data;
using MyShopPet.Infrastructures.ModelBinderProviders;
using MyShopPet.Repositories.Abstraction;
using MyShopPet.Repositories;
using MyShopPet.Services;
using MyShopPet.Services.Abstraction;

namespace MyShopPet.Extensions
{
    public static class StartupExtensions
    {
        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
        {
            var configuration = builder.Configuration;
            builder.Services.AddControllersWithViews(options =>
            options.ModelBinderProviders.Insert(0, new CartModelBinderProvider()));
            builder.Services.AddScoped<IProductRepository, ProductRepository>().
            AddScoped<ICategoryRepository, CategoryRepository>().
            AddScoped<IPhotoRepository, PhotoRepository>().
            AddScoped<IFileSavingHandler, FileSavingHandler>().
            AddScoped<IUniquePathGenerator, UniquePathGenerator>();

            builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ShopDbContext>();
            builder.Services.AddAutoMapper(typeof(UserProfile), typeof(CreateProductProfile));
            string connStr = builder.Configuration.GetConnectionString("shopDb" ?? throw new InvalidOperationException("Connection string not set!"));
            builder.Services.AddDbContext<ShopDbContext>(options =>
            options.UseSqlServer(connStr));
            builder.Services.AddAuthentication().AddGoogle(options =>
            {
                IConfiguration googleSection = configuration.GetSection("Authentication:Google");
                options.ClientId = googleSection.GetValue<string>("ClientId");
                options.ClientSecret = googleSection["ClientSecret"];
            }).AddFacebook(options =>
            {
                var section = configuration.GetSection("Authentication:Facebook");
                options.AppId = section["AppId"];
                options.AppSecret = section["AppSecret"];
            });
            builder.Services.AddAuthorization(option =>
            {
                option.AddPolicy("Admin policy", policy =>
                {
                    policy.RequireRole("admin", "manager");
                });
            });
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();
            builder.Services.Configure<IdentityOptions>(options => {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
            });
            return builder;
        }
        public static async Task Configure(this WebApplication app)
        {
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var rolesManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                await RoleInitializer.InitializeAsync(userManager, rolesManager, configuration);
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();
            app.MapControllerRoute(
               name: "claimsIndex",
               pattern: "/Claims",
               defaults: new { controller = "Claims", action = "Index" });
            app.MapControllerRoute(
               name: "rolesIndex",
               pattern: "/Roles",
               defaults: new { controller = "Roles", action = "Index" });
            app.MapControllerRoute(
                name: "cartIndex",
                pattern: "/Cart",
                defaults: new { controller = "Cart", action = "Index" });
            app.MapControllerRoute(
                name: "addToCard",
                pattern: "/Cart/{id}/{returnUrl}",
                defaults: new { controller = "Cart", action = "AddToCard" });
            app.MapControllerRoute(
                name: "adminIndex",
                pattern: "/Admin",
                defaults: new { controller = "Admin", action = "Index" });
            app.MapControllerRoute(
                name: "userIndex",
                pattern: "/User",
                defaults: new { controller = "User", action = "Index" });
            app.MapControllerRoute(
                name: "homeIndex",
                pattern: "/",
                defaults: new { controller = "Home", action = "Index" });
            app.MapControllerRoute(
                name: "homeIndexPage",
                pattern: "/page{page:int}",
                defaults: new { controller = "Home", action = "Index" });
            app.MapControllerRoute(
                name: "category",
                pattern: "/{category?}",
                defaults: new { controller = "Home", action = "Index" });
            app.MapControllerRoute(
                name: "categoryPage",
                pattern: "/{category}/page{page:int}",
                defaults: new { controller = "Home", action = "Index" });
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        }
    }
}
