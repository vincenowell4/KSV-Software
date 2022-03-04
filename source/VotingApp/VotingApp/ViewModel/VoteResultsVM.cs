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
    } 
}
