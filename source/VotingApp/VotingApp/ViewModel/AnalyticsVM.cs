using VotingApp.Models;

namespace VotingApp.ViewModel
{
    public class AnalyticsVM
    {
        public string VoteTitle { get; set; }
        public string VoteDescription { get; set; }

        public IList<VoteOption> VoteOptions { get; set; }
        public IList<string> ChartVoteOptions { get; set; }
        public IList<int> ChartVoteTotals { get; set; }
    }
}
