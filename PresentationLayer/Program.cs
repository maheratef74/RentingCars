using System.Globalization;
using System.Text.Json;
using BusinessLogicLayer.Jobs;
using BusinessLogicLayer.Services.File;
using DataAccessLayer.DbContext;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Car;
using DataAccessLayer.Repositories.Renting;
using DataAccessLayer.Repositories.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using RentingCars.Controllers;
using RentingCars.Localization;

var builder = WebApplication.CreateBuilder(args);
#region DataBase Config
        
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options
        .UseSqlServer(connectionString)
        .LogTo(Console.WriteLine, LogLevel.Information);
});
#endregion

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<IRentingRepository, RentingRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<RoleSeeder>();
builder.Services.AddHostedService<RentalCleanupService>();

#region Add Identity services
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.Password.RequireDigit = false;            // No digit required
        options.Password.RequiredLength = 6;              // Minimum length of 6
        options.Password.RequireNonAlphanumeric = false;  // No special character required
        options.Password.RequireUppercase = false;        // No uppercase letter required
        options.Password.RequireLowercase = false;        // No lowercase letter required
        options.Password.RequiredUniqueChars = 1;
    })
    .AddEntityFrameworkStores<AppDbContext>()  
    .AddDefaultTokenProviders();  
        
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/auth/Login"; // Redirect here if not authenticated
    options.AccessDeniedPath = "/Home/Error"; // Optional path for denied access
    options.SlidingExpiration = true; // Extend cookie expiration on each request
    options.ExpireTimeSpan = TimeSpan.FromDays(14);
});
#endregion 
             
builder.Services.AddMvc(options =>
{
    options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(
        _ => "This field is required.");
}).AddViewOptions(options =>
{
    options.HtmlHelperOptions.ClientValidationEnabled = true;
});
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.IgnoreNullValues = true; // Example option
    });
#region localization

builder.Services.AddLocalization();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSingleton<IStringLocalizerFactory, JSonStringLocalizerFactory>();
builder.Services.AddMvc()
    .AddControllersAsServices()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization(  options =>
        {
            options.DataAnnotationLocalizerProvider = (type , factory) =>
                factory.Create(typeof(JSonStringLocalizerFactory));
        }
    );
builder.Services.Configure<RequestLocalizationOptions>(options =>
    {
        var supportedCultures = new List<CultureInfo>
        {
            new CultureInfo("ar-EG"),
            new CultureInfo("en-US")
        };
        options.DefaultRequestCulture = new RequestCulture(culture: supportedCultures[0] , uiCulture: supportedCultures[0] ); // arabic
        options.SupportedCultures = supportedCultures;
        options.SupportedUICultures = supportedCultures;
    }
);
#endregion

var app = builder.Build();


#region Role Seeding

// Resolve RoleSeeder from DI and seed roles
using (var scope = app.Services.CreateScope())
{
    var roleSeeder = scope.ServiceProvider.GetRequiredService<RoleSeeder>();
    await roleSeeder.SeedRolesAndAdminAsync();
}

#endregion

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
#region localization
var supportedCultures = new[] { "ar-EG", "en-US" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);
app.UseRequestLocalization(localizationOptions);
#endregion

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=auth}/{action=login}/{id?}");

app.Run();