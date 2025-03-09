using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AdultGamingForum.Data;
using AdultGamingForum.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ForumContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("ForumContext")
        ?? throw new InvalidOperationException("Connection string 'ForumContext' not found."),
        sqlOptions => {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,               // The maximum number of retry attempts
                maxRetryDelay: TimeSpan.FromSeconds(30), // The maximum delay between retries
                errorNumbersToAdd: null         // Optionally, specify additional SQL error numbers that should trigger a retry
            );
        }
    ));
// updated to use the new Identity scaffolding. 9:37pm 3/6/2025
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<ForumContext>();


// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();
// Identity requires the following routes for login, logout, etc.
app.MapRazorPages().WithStaticAssets();

app.Run();
