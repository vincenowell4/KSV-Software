using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace VotingAppService
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
     // Delayed Vote Check ============================================================================================================
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

    // Multi-Round Vote Check =============================================================================================================

                // get all rows in CreatedVotes  where VoteType is 3, the ClosedDate is n the past, and RoundOver is false
                HttpRequestMessage request2 = new HttpRequestMessage(HttpMethod.Get, client.BaseAddress + "cmv/all");
                request2.Headers.Add(APIKEYNAME, apiKey);
                HttpResponseMessage response2 = await client.SendAsync(request2);
                _logger.LogInformation("Response code: " + response2.StatusCode);
                if (response2.IsSuccessStatusCode)
                {
                    string createdMultiRoundVotesJsonStr = await response2.Content.ReadAsStringAsync(stoppingToken);
                    _logger.LogInformation(createdMultiRoundVotesJsonStr);

                    if (createdMultiRoundVotesJsonStr.Length > 0) // there are possibly multi-round votes to process
                    {
                        List<CreatedMultiRoundVoteViewModel> cmrvList = JsonConvert.DeserializeObject<List<CreatedMultiRoundVoteViewModel>>(createdMultiRoundVotesJsonStr);

                        if (cmrvList.Count > 0) // there are possibly multi-round votes to process
                        {
                            for (int i = 0; i < cmrvList.Count; i++)
                            {
                                //given a vote id, return each option from the VoteOptions table for that vote, as well as the total votes for 
                                //that option from SubmittedVotes, so the perecentages for each option can be determined
                                request2 = new HttpRequestMessage(HttpMethod.Get, client.BaseAddress + "vr/" + cmrvList[i].Id.ToString());
                                request2.Headers.Add(APIKEYNAME, apiKey);
                                response2 = await client.SendAsync(request2);
                                _logger.LogInformation("Response code: " + response2.StatusCode);
                                if (response2.IsSuccessStatusCode)
                                {
                                    string multiRoundVoteResultsJsonStr = await response2.Content.ReadAsStringAsync(stoppingToken);
                                    _logger.LogInformation(multiRoundVoteResultsJsonStr);

                                    List<MultiRoundVoteResultsModel> mrvrList = JsonConvert.DeserializeObject<List<MultiRoundVoteResultsModel>>(multiRoundVoteResultsJsonStr);
                                    int totalVotes = 0;

                                    if (mrvrList.Count > 0) 
                                    {
                                        for (int j= 0;j < mrvrList.Count;j++)
                                        {
                                            totalVotes += mrvrList[j].VoteOptCount;
                                        }

                                        double topVoteOptionPercent = 0;
                                        if (totalVotes > 0)
                                            topVoteOptionPercent = mrvrList[0].VoteOptCount * 100 / totalVotes;

                                       if (topVoteOptionPercent > 50)
                                        {
                                            //scenario 0: no matter how many vote option there were, one option has more that 50% (a majority) of the votes
                                            //      in this scenario, the vote with the majority is the winner - notify the vote creator of the results

                                            _logger.LogInformation("Vote option: [" + mrvrList[0].VoteOpt + "] received " + topVoteOptionPercent + "% of vote; it is the winner");

                                            //set NextVoteId to -1, so we won't process this vote again
                                            request2 = new HttpRequestMessage(HttpMethod.Get, client.BaseAddress + "setvotedone/" + cmrvList[i].Id.ToString());
                                            request2.Headers.Add(APIKEYNAME, apiKey);
                                            response2 = await client.SendAsync(request2);
                                            _logger.LogInformation("Response code: " + response2.StatusCode);
                                            if (response2.IsSuccessStatusCode)
                                            {
                                                string voteStatus = await response2.Content.ReadAsStringAsync(stoppingToken);
                                                if (voteStatus == "Done")
                                                    _logger.LogInformation("Closed multi-round vote ID: " + cmrvList[i].Id.ToString());
                                                else
                                                    _logger.LogError("Couldn't close multi-round vote ID: " + cmrvList[i].Id.ToString());
                                            }

                                            request2 = new HttpRequestMessage(HttpMethod.Get, client.BaseAddress + "votewonemail/" + cmrvList[i].Id.ToString());
                                            request2.Headers.Add(APIKEYNAME, apiKey);
                                            response2 = await client.SendAsync(request2);

                                            string email = await response2.Content.ReadAsStringAsync(stoppingToken);
                                            _logger.LogInformation("email sent to: " + email);

                                        } else // there isn't a majority winning vote option
                                        {
                                            if (totalVotes > 0)
                                            {
                                                if (mrvrList.Count > 1)
                                                {
                                                    int lastOption = mrvrList.Count - 1;
                                                    if (mrvrList[lastOption].VoteOptCount == mrvrList[lastOption - 1].VoteOptCount)
                                                    {
                                                        //scenario 2: there are more than 2 options, none has a majority, but there is a tie for last place
                                                        //            OR there are only 2 options, but they both have 50% of the vote
                                                        //            in other words, there is a tie for last place, so no clear option to eliminate
                                                        //      in this scenario, do not create a new vote - just notify vote creator of vote options and percentages

                                                        _logger.LogInformation("There is a tie for a winner or a tie for last place vote options");

                                                        //set NextVoteId to -1, so we won't process this vote again
                                                        request2 = new HttpRequestMessage(HttpMethod.Get, client.BaseAddress + "setvotedone/" + cmrvList[i].Id.ToString());
                                                        request2.Headers.Add(APIKEYNAME, apiKey);
                                                        response2 = await client.SendAsync(request2);
                                                        _logger.LogInformation("Response code: " + response2.StatusCode);
                                                        if (response2.IsSuccessStatusCode)
                                                        {
                                                            string voteStatus = await response2.Content.ReadAsStringAsync(stoppingToken);
                                                            if (voteStatus == "Done")
                                                                _logger.LogInformation("Closed multi-round vote ID: " + cmrvList[i].Id.ToString());
                                                            else
                                                                _logger.LogError("Couldn't close multi-round vote ID: " + cmrvList[i].Id.ToString());
                                                        }

                                                        request2 = new HttpRequestMessage(HttpMethod.Get, client.BaseAddress + "votetieemail/" + cmrvList[i].Id.ToString());
                                                        request2.Headers.Add(APIKEYNAME, apiKey);
                                                        response2 = await client.SendAsync(request2);

                                                        string email = await response2.Content.ReadAsStringAsync(stoppingToken);
                                                        _logger.LogInformation("email sent to: " + email);

                                                    } else //create another round of voting and drop the vote option with the least votes
                                                    {
                                                        //scenario 1: there are more than 2 vote options, none has a majority, and there is not a tie for last place
                                                        //      in this scenario, create a new vote, and create new options for this vote, removing the lowest percentage option

                                                        _logger.LogInformation("Creating another round for vote: " + cmrvList[i].Id.ToString());

                                                        request2 = new HttpRequestMessage(HttpMethod.Get, client.BaseAddress + "nextround/" + cmrvList[i].Id.ToString());
                                                        request2.Headers.Add(APIKEYNAME, apiKey);
                                                        response2 = await client.SendAsync(request2);

                                                        string newVoteId = await response2.Content.ReadAsStringAsync(stoppingToken);

                                                        if (newVoteId.Length > 0) {
                                                            _logger.LogInformation("Next Round - new VoteId: " + newVoteId);
                                                            request2 = new HttpRequestMessage(HttpMethod.Get, client.BaseAddress + "nextroundemail/" + newVoteId);
                                                            request2.Headers.Add(APIKEYNAME, apiKey);
                                                            response2 = await client.SendAsync(request2);

                                                            string email = await response2.Content.ReadAsStringAsync(stoppingToken);
                                                            _logger.LogInformation("email sent to: " + email);
                                                        } else
                                                        {
                                                            _logger.LogInformation("Next Round - no vote ID returned");
                                                        }
                                                    }

                                                } else //there was only one option left - something went wrong
                                                {
                                                    _logger.LogError("Error getting vote results - only one vote option was returned");
                                                }
                                            } else // there were no submitted votes 
                                            {
                                                _logger.LogInformation("There are zero votes for all vote options");

                                                //set NextVoteId to -1, so we won't process this vote again
                                                request2 = new HttpRequestMessage(HttpMethod.Get, client.BaseAddress + "setvotedone/" + cmrvList[i].Id.ToString());
                                                request2.Headers.Add(APIKEYNAME, apiKey);
                                                response2 = await client.SendAsync(request2);
                                                _logger.LogInformation("Response code: " + response2.StatusCode);
                                                if (response2.IsSuccessStatusCode)
                                                {
                                                    string voteStatus = await response2.Content.ReadAsStringAsync(stoppingToken);
                                                    if(voteStatus == "Done")
                                                        _logger.LogInformation("Closed multi-round vote ID: " + cmrvList[i].Id.ToString());
                                                    else
                                                        _logger.LogError("Couldn't close multi-round vote ID: " + cmrvList[i].Id.ToString());
                                                }

                                                //notify vote creator that there were no submitted votes
                                                request2 = new HttpRequestMessage(HttpMethod.Get, client.BaseAddress + "zerovotesemail/" + cmrvList[i].Id.ToString());
                                                request2.Headers.Add(APIKEYNAME, apiKey);
                                                response2 = await client.SendAsync(request2);

                                                string email = await response2.Content.ReadAsStringAsync(stoppingToken);
                                                _logger.LogInformation("email sent to: " + email);
                                            }
                                        }
                                        totalVotes = 0;
                                    }
                                    else //there was an error getting the vote results
                                    {
                                        _logger.LogError("Error getting vote results - no vote options were returned");
                                    }
                                }
                            }
                        }
                    }
                }
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                //await Task.Delay(3600000, stoppingToken);
                await Task.Delay(5000, stoppingToken);
            }
        }
    }

    public class CreatedVoteViewModel
    {
        public int Id { get; set; }
        public DateTime? VoteOpenDateTime { get; set; }
    }

    public class CreatedMultiRoundVoteViewModel
    {
        public int Id { get; set; }
    }

    public class VoteResultsViewModel
    {
        public int VoteOptionId { get; set; }
        public string VoteOptionString { get; set; }
        public int VoteOptionVoteCount {  get; set; }
    }

    public class MultiRoundVoteResultsModel
    {
        public string VoteOpt { get; set; }
        public int VoteOptCount { get; set; }
    }
}