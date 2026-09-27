using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using FanHubPlus.Data;
using FanHubPlus.Services;
using FanHubPlus.Hubs;

var builder = WebApplication.CreateBuilder(args);

// MVC services
builder.Services.AddControllersWithViews(options =>
{
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
});

// Adds "Views/Partials" as an additional Razor view-location
builder.Services.Configure<RazorViewEngineOptions>(options =>
{
    options.ViewLocationFormats.Add("/Views/Partials/{0}.cshtml");
});

// Database context + connection string registration
builder.Services.AddDbContext<FanHubContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FanHubContext")));

// Identity, using ApplicationUser instead of the default IdentityUser
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<FanHubContext>();

// Re-check the security stamp every minute so a blocked user or a changed role takes effect quickly
builder.Services.Configure<SecurityStampValidatorOptions>(options =>
    options.ValidationInterval = TimeSpan.FromMinutes(1));

// SMTP email sender
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("Smtp"));
builder.Services.AddTransient<IEmailSender, EmailSender>();

// Cloudinary service
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();


// SignalR hubs
builder.Services.AddSignalR();

var app = builder.Build();

// Migrate Database & Seed Initial Fandom Data
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var context = scope.ServiceProvider.GetRequiredService<FanHubContext>();

    // Ensure database schema and migrations are applied to SQL Server
    try
    {
        await context.Database.MigrateAsync();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database migration note: {ex.Message}");
        await context.Database.EnsureCreatedAsync();
    }

    // Ensure application roles exist
    string[] roles = { "Admin", "Customer" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }

    // Seed all fandom data, categories, characters, articles, events, merchandise, polls, FAQs
    await SeedData.SeedAllDataAsync(context);

    // Seed default admin account
    string adminEmail = "admin@demo.com";
    string adminpass = "Admin@123";

    if (await userManager.FindByEmailAsync(adminEmail) == null)
    {
        var adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(adminUser, adminpass);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

// SignalR hub endpoints
app.MapHub<PollHub>("/hubs/poll");
app.MapHub<EventChatHub>("/hubs/eventchat");
app.MapHub<NotificationHub>("/hubs/notifications");

app.Run();
