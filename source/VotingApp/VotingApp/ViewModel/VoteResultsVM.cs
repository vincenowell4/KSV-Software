using VotingApp.Models;
namespace VotingApp.ViewModel
{
    public class VoteResultsVM
    {
        public string VoteTitle { get; set; }
        public string VoteDescription { get; set; }

        public int VoteId { get; set; }
        public IList<VoteOption> VoteOptions { get; set; }

        public Dictionary<VoteOption, int> TotalVotesForEachOption { get; set; }
        public bool AnonymousVote { get; set; }

        public Dictionary<VoteOption, SubmittedVote> VotesForLoggedInUsers { get; set; }
        public Dictionary<VoteOption, int> VotesForUsersNotLoggedIn { get; set; }
        public int TotalVotesCount { get; set; }

        public Dictionary<VoteOption, int> Winners { get; set; }

        public CreatedVote GetVoteByAccessCode { get; set; }
        public IList<string> ChartVoteOptions { get; set; }
        public IList<int> ChartVoteTotals { get; set; }
    } 
}
