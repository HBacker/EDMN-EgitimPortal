using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EgitimPortalFinal.Localisation;
using EgitimPortalFinal.Models;
using AspNetCoreHero.ToastNotification;
using Microsoft.Extensions.FileProviders;
using EgitimPortalFinal.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()));

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("SqlCon");

    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException("SQL connection string is missing: 'SqlCon'.");
    }

    options.UseSqlServer(connectionString);
});

builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 10;
    config.IsDismissable = true;
    config.Position = NotyfPosition.BottomRight;
});

builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
    options.TokenLifespan = TimeSpan.FromHours(2);
});

builder.Services.AddIdentity<AppUser, AppRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequireUppercase = true;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
    options.Lockout.MaxFailedAccessAttempts = 3;
})
.AddDefaultTokenProviders()
.AddErrorDescriber<ErrorDescription>()
.AddEntityFrameworkStores<AppDbContext>();

builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.LoginPath = "/Home/Login";
    opt.LogoutPath = "/Member/Logout";
    opt.AccessDeniedPath = "/Home/AccessDenied";

    opt.Cookie = new CookieBuilder
    {
        Name = "mvcUID",
        HttpOnly = true,
        SameSite = SameSiteMode.Lax,
    };

    opt.ExpireTimeSpan = TimeSpan.FromDays(15);
    opt.SlidingExpiration = true;
});

// Add custom services
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAPIRepository, APIRepository>();  // Add the API repository

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Course}/{action=Course}/{id?}");

app.Run();
