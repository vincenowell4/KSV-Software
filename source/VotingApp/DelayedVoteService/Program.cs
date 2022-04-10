using DelayedVoteService;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
string vaApiKey = "";

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        vaApiKey = builder.Configuration["VotingAppApiKey"];
    }
    catch (Exception ex)
    {
    }
}

ApiKey.voteAppApiKey = vaApiKey; //set the API key in a static class so it can be seen from Worker.cs

IHost host = Host.CreateDefaultBuilder(args)
.ConfigureServices(services =>
{
    services.AddHostedService<Worker>();
})
.Build();

await host.RunAsync();
