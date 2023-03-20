using MyShopPet.AutoMapperProfile;
using Microsoft.EntityFrameworkCore;
using MyShopPet.Data;
using MyShopPet.Interfaces;
using MyShopPet.Repositories;
using Microsoft.AspNetCore.Identity;
using MyShopPet.Infrastructures.ModelBinderProviders;
using MyShopPet.Services;
using MyShopPet.Middleware;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
builder.Services.AddControllersWithViews(options =>
options.ModelBinderProviders.Insert(0, new CartModelBinderProvider()));
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
builder.Services.AddTransient<IPhotoRepository, PhotoRepository>();
builder.Services.AddIdentity<IdentityUser,IdentityRole>().AddEntityFrameworkStores<ShopDbContext>();
builder.Services.AddAutoMapper(typeof(UserProfile));
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
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var rolesManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await RoleInitializer.InitializeAsync(userManager, rolesManager, configuration);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseMiddleware<ExeptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "/Cart",
    defaults: new { controller = "Cart", action = "Index" });
app.MapControllerRoute(
    name: "default",
    pattern: "/Cart/{id}/{returnUrl}",
    defaults: new { controller = "Cart", action = "AddToCard" });
app.MapControllerRoute(
    name: "default",
    pattern: "/Admin",
    defaults: new { controller = "Admin", action = "Index" });
app.MapControllerRoute(
    name: "default",
    pattern: "/User",
    defaults: new { controller = "User", action = "Index" });
app.MapControllerRoute(
    name: "default",
    pattern: "/",
    defaults: new { controller = "Home", action = "Index" });
app.MapControllerRoute(
    name: "default",
    pattern: "/page{page:int}",
    defaults: new { controller = "Home", action = "Index" });
app.MapControllerRoute(
    name: "default",
    pattern: "/{category?}",
    defaults: new { controller = "Home", action = "Index" });
app.MapControllerRoute(
    name: "default",
    pattern: "/{category}/page{page:int}",
    defaults: new { controller = "Home", action = "Index" });
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
