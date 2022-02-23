using VotingApp.DAL.Abstract;
using VotingApp.DAL.Concrete;
using VotingApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VotingApp.Data;
using VotingApp.Utilities;
//using VotingApp.Data;
using static VotingApp.Utilities.SeedUser;
using EmailService;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// WHEN RUNNING LOCALLY AGAINST A LOCAL DATABASE, USE THIS
var connectionStringIdentity = builder.Configuration.GetConnectionString("VotingAppIdentity");

// WHEN RUNNING LOCALLY AGAINST AN AZURE DATABASE, USE THIS
//var connectionStringIdentity = new SqlConnectionStringBuilder(builder.Configuration.GetConnectionString("VotingAppIdentityAzure"));
//connectionStringIdentity.Password = builder.Configuration["VotingApp:CSpwd"];

// JUST BEFORE DEPLOYING THE APP TO AZURE, COMMENT OUT THE ABOVE LINES AND UNCOMMENT THIS LINE  - THIS WILL USE APPSETTINGS ON AZURE
//var connectionStringIdentity = builder.Configuration.GetConnectionString("VotingAppIdentityAzure");


//var connectionStringIdentity = new SqlConnectionStringBuilder(builder.Configuration.GetConnectionString("VotingAppIdentityAzure"));
//if (connectionStringIdentity.Password.Length == 0)
  //  connectionStringIdentity.Password = builder.Configuration["VotingApp:CSpwd"];

var emailConfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
emailConfig.UserName = builder.Configuration["EmailUserName"];
emailConfig.Password = builder.Configuration["EmailPassword"];
builder.Services.AddSingleton(emailConfig);
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddDbContext<VotingAppIdentityContext>(options =>options.UseSqlServer(connectionStringIdentity.ToString()));
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<VotingAppIdentityContext>();

// Add services to the container.

// WHEN RUNNING LOCALLY AGAINST A LOCAL DATABASE, USE THIS
var connectionString = builder.Configuration.GetConnectionString("VotingAppConnection");

// WHEN RUNNING LOCALLY AGAINST AN AZURE DATABASE, USE THIS
//var connectionString = new SqlConnectionStringBuilder(builder.Configuration.GetConnectionString("VotingAppConnectionAzure"));
//connectionString.Password = builder.Configuration["VotingApp:CSpwd"];

// JUST BEFORE DEPLOYING THE APP TO AZURE, COMMENT OUT THE ABOVE LINES AND UNCOMMENT THIS LINE  - THIS WILL USE APPSETTINGS ON AZURE
//var connectionString = builder.Configuration.GetConnectionString("VotingAppConnectionAzure");

//var connectionString = new SqlConnectionStringBuilder(builder.Configuration.GetConnectionString("VotingAppConnectionAzure"));
//if (connectionString.Password.Length == 0)
  //  connectionString.Password = builder.Configuration["VotingApp:CSpwd"];

builder.Services.AddDbContext<VotingAppDbContext>(options =>
    options.UseSqlServer(connectionString.ToString()));

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddScoped<DbContext, VotingAppDbContext>();
builder.Services.AddScoped<ICreatedVoteRepository, CreatedVoteRepository>();
builder.Services.AddScoped<IVoteTypeRepository, VoteTypeRepository>();
builder.Services.AddScoped<VoteCreationService, VoteCreationService>();


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
