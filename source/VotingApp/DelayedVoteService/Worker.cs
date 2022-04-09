using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace DelayedVoteService
{
    public class Worker : BackgroundService
    {
        static HttpClient client = new HttpClient();
        private readonly ILogger<Worker> _logger;
        private const string APIKEYNAME = "ApiKey";

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<Program>().Build();
            var section = config.GetSection(nameof(ApiClientConfiguration));
            var apiClientConfig = section.Get<ApiClientConfiguration>();
            _logger.LogInformation("VotingAppApiUrl: " + apiClientConfig.VotingAppApiUrl);

            string apiKey = ApiKey.voteAppApiKey;

            client.BaseAddress = new Uri(apiClientConfig.VotingAppApiUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            while (!stoppingToken.IsCancellationRequested)
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, client.BaseAddress + "novac/all");
                request.Headers.Add(APIKEYNAME, apiKey);
                HttpResponseMessage response = await client.SendAsync(request);
                _logger.LogInformation("Response code: " + response.StatusCode);
                if (response.IsSuccessStatusCode)
                {
                    string createdVotesJsonStr = await response.Content.ReadAsStringAsync(stoppingToken);
                    _logger.LogInformation(createdVotesJsonStr);

                    if (createdVotesJsonStr.Length > 0)
                    {
                        try
                        {
                            List<CreatedVoteViewModel> createdVotes = JsonConvert.DeserializeObject<List<CreatedVoteViewModel>>(createdVotesJsonStr);

                            foreach (CreatedVoteViewModel createdVote in createdVotes)
                            {
                                if (createdVote.VoteOpenDateTime != null && DateTime.Compare(createdVote.VoteOpenDateTime.Value, DateTime.Now) < 0 )
                                {
                                    // The vote open date/time for this vote is in the past - create an access code
                                    _logger.LogInformation("Created Vote ID: " + createdVote.Id + " opened " + createdVote.VoteOpenDateTime);
                                    _logger.LogInformation("Creating Vote Access Code for Vote ID: " + createdVote.Id);

                                    request = new HttpRequestMessage(HttpMethod.Get, client.BaseAddress + "genvac/" + createdVote.Id.ToString());
                                    request.Headers.Add(APIKEYNAME, apiKey);
                                    response = await client.SendAsync(request);

                                    string accCode = await response.Content.ReadAsStringAsync(stoppingToken);
                                    if (accCode.Length > 0)
                                    {
                                        _logger.LogInformation(accCode);

                                        //access code created - send email to vote creator with vote access code in the email
                                        request = new HttpRequestMessage(HttpMethod.Get, client.BaseAddress + "genemail/" + createdVote.Id.ToString());
                                        request.Headers.Add(APIKEYNAME, apiKey);
                                        response = await client.SendAsync(request);

                                        string email = await response.Content.ReadAsStringAsync(stoppingToken);
                                        _logger.LogInformation("email sent to: " + email);
                                    }
                                    else
                                        _logger.LogInformation("no access code returned");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError("Exception caught: " + ex.Message);
                        }
                    }
                }
                else
                {
                    _logger.LogError("API response error: " + response.ReasonPhrase);
                }
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(3600000, stoppingToken);
            }
        }
    }

    public class CreatedVoteViewModel
    {
        public int Id { get; set; }
        public DateTime? VoteOpenDateTime { get; set; }
    }
}