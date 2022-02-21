using VotingApp.Models;

namespace VotingApp.ViewModel
{
    public class SubmitVoteVM
    {
        public CreatedVote vote { get; set; }
        public List<string> options { get; set; }

    }
}
