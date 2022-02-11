using VotingApp.DAL.Abstract;
using VotingApp.DAL.Concrete;
using VotingApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VotingApp.Data;
using VotingApp.Utilities;
using VotingApp.Data;
using static VotingApp.Utilities.SeedUser;

var builder = WebApplication.CreateBuilder(args);
var connectionStringIdentity = builder.Configuration.GetConnectionString("VotingAppIdentity");
builder.Services.AddDbContext<VotingAppIdentityContext>(options =>options.UseSqlServer(connectionStringIdentity));
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<VotingAppIdentityContext>();

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("VotingAppConnection");
builder.Services.AddDbContext<VotingAppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddScoped<DbContext, VotingAppDbContext>();
builder.Services.AddScoped<ICreatedVoteRepository, CreatedVoteRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // Get the IConfiguration service that allows us to query user-secrets and 
        // the configuration on Azure
        // Set password with the Secret Manager tool, or store in Azure app configuration
        // dotnet user-secrets set SeedUserPW <pw>
        var testUserPw = builder.Configuration["SeedUserPW"];
        var adminPw = builder.Configuration["SeedAdminPW"];
       

        SeedUsers.Initialize(services, SeedData.UserSeedData, testUserPw).Wait();
        SeedUsers.InitializeAdmin(services, "admin@example.com", "admin", adminPw, "The", "Admin").Wait();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                    app.MapRazorPages();

app.Run();
