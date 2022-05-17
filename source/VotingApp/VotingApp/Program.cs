using VotingApp.DAL.Abstract;
using VotingApp.DAL.Concrete;
using VotingApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VotingApp.Data;
using static VotingApp.Utilities.SeedUser;
using EmailService;
using Google.Apis.Auth.OAuth2;
using Microsoft.Data.SqlClient;
using System.Reflection;

// Add services to the container.

var builder = WebApplication.CreateBuilder(args);

// WHEN RUNNING LOCALLY AGAINST A LOCAL DATABASE, USE THIS
var connectionStringIdentity = builder.Configuration.GetConnectionString("VotingAppIdentity");

//*******************************************************************************************************************************************
//IMPORTANT - USE THE NEXT 3 LINES OF CODE WHEN DEPLOYING TO THE DEMO SITE, 
//var connectionStringIdentity = new SqlConnectionStringBuilder(builder.Configuration.GetConnectionString("VotingAppIdentityDemoAzure"));
//if (connectionStringIdentity.Password.Length == 0)
//    connectionStringIdentity.Password = builder.Configuration["VotingApp:CSpwd"];
//OTHERWISE USE THESE THREE LINES OF CODE WHEN RUNNING LOCALLY AGAINST AN AZURE DATABASE
//var connectionStringIdentity = new SqlConnectionStringBuilder(builder.Configuration.GetConnectionString("VotingAppIdentityAzure"));
//if (connectionStringIdentity.Password.Length == 0)
//    connectionStringIdentity.Password = builder.Configuration["VotingApp:CSpwd"];
//var connectionStringIdentity = new SqlConnectionStringBuilder(builder.Configuration.GetConnectionString("SamsTestIdentityVotingApp"));
//if (connectionStringIdentity.Password.Length == 0)
//connectionStringIdentity.Password = builder.Configuration["SamAzureIdentityTestPW"];
//*******************************************************************************************************************************************

var emailConfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
emailConfig.UserName = builder.Configuration["EmailUserName"];
emailConfig.Password = builder.Configuration["EmailPassword"];
builder.Services.AddSingleton(emailConfig);
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddDbContext<VotingAppIdentityContext>(options =>options.UseSqlServer(connectionStringIdentity.ToString()));
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<VotingAppIdentityContext>();

var binDirectory = Path.GetDirectoryName(Assembly.GetCallingAssembly().CodeBase);
string fullPath = Path.Combine(binDirectory, "credentials.json").Replace("file:\\", "");

using (StreamWriter outputFile = new StreamWriter(fullPath, false))
{
    outputFile.WriteLine(builder.Configuration["GOOGLECREDS"]);
}

// Set environment variabel to the full file path
Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", fullPath);

// WHEN RUNNING LOCALLY AGAINST A LOCAL DATABASE, USE THIS
var connectionString = builder.Configuration.GetConnectionString("VotingAppConnection");
//*******************************************************************************************************************************************
//IMPORTANT - USE THE NEXT 3 LINES OF CODE WHEN DEPLOYING TO THE DEMO SITE, 
//var connectionString = new SqlConnectionStringBuilder(builder.Configuration.GetConnectionString("VotingAppDemoConnectionAzure"));
//if (connectionString.Password.Length == 0)
//    connectionString.Password = builder.Configuration["VotingApp:CSpwd"];
//OTHERWISE USE THESE THREE LINES OF CODE WHEN RUNNING LOCALLY AGAINST AN AZURE DATABASE
//var connectionString = new SqlConnectionStringBuilder(builder.Configuration.GetConnectionString("VotingAppConnectionAzure"));
//if (connectionString.Password.Length == 0)
//    connectionString.Password = builder.Configuration["VotingApp:CSpwd"];
//var connectionString = new SqlConnectionStringBuilder(builder.Configuration.GetConnectionString("SamsTestVotingApp"));
//if (connectionString.Password.Length == 0)
// connectionString.Password = builder.Configuration["SamAzureTestPW"];
//*******************************************************************************************************************************************

builder.Services.AddDbContext<VotingAppDbContext>(options =>
    options.UseLazyLoadingProxies().UseSqlServer(connectionString.ToString()));

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddScoped<DbContext, VotingAppDbContext>();
builder.Services.AddScoped<ICreatedVoteRepository, CreatedVoteRepository>();
builder.Services.AddScoped<IVoteTypeRepository, VoteTypeRepository>();
builder.Services.AddScoped<IVoteOptionRepository, VoteOptionRepository>();
builder.Services.AddScoped<IVotingUserRepositiory, VotingUserRepository>();
builder.Services.AddScoped<IVoteOptionRepository, VoteOptionRepository>();
builder.Services.AddScoped<VoteCreationService, VoteCreationService>();
builder.Services.AddScoped<CreationService, CreationService>();
builder.Services.AddScoped<ISubmittedVoteRepository, SubmittedVoteRepository>();
builder.Services.AddScoped<IVoteAuthorizedUsersRepo, VoteAuthorizedUsersRepo>();
builder.Services.AddScoped<GoogleTtsService,GoogleTtsService>();
builder.Services.AddScoped<IAppLogRepository, AppLogRepository>();
builder.Services.AddScoped<ITimeZoneRepo, TimeZoneRepo>();

builder.Services.Configure<DataProtectionTokenProviderOptions>(o =>
       o.TokenLifespan = TimeSpan.FromSeconds(06400)); //email confirmation token will expire after exactly 24 hours (86400 seconds)

var app = builder.Build();

app.Use(async (ctx, next) =>
{
    await next();

    if (ctx.Response.StatusCode == 404 && !ctx.Response.HasStarted)
    {
        //Re-execute the request so the user gets the error page
        string originalPath = ctx.Request.Path.Value;
        ctx.Items["originalPath"] = originalPath;
        ctx.Request.Path = "/error/404";
        await next();
    }
});
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error/500");
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
        //var apiKey = builder.Configuration["GoogleAPIKey"];
        //Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", apiKey);
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
app.MapControllerRoute(
        name: "Access Vote Share Link",
        pattern: "Access/{code?}",
        defaults: new { controller = "Access", action = "Access" });
    
app.Run();