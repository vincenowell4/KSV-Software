using VotingApp.Models;

namespace VotingApp.ViewModel
{
    public class SubmitVoteVM
    {
        public CreatedVote vote { get; set; }
        public List<VoteOption> options { get; set; }

        public SubmittedVote submittedVote { get; set; }

    }
}
