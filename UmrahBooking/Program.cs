using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UmrahBooking.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DataBaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("UmrahBooking")));

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    // Configure Identity options here
})
.AddEntityFrameworkStores<DataBaseContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
})
    .AddCookie();

builder.Services.ConfigureApplicationCookie(options => options.LoginPath = "/SignIn/Index");

builder.Services.AddLogging(builder =>
{
    builder.AddConsole(); // You can add other providers like Debug, EventLog, etc.
});
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
//    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
//    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
//})
//.AddIdentityCookies(options => { });

//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
//});



builder.Services.AddAuthentication();

builder.Services.Configure<IdentityOptions>(options =>
{

    // Configure Customize password requirements, lockout settings, etc.

});

builder.Services.AddAuthorization();
builder.Services.AddHttpClient();
builder.Services.AddSession();

var app = builder.Build();
app.UseSession();
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
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=SignUp}/{action=Index}/{id?}");

app.Run();