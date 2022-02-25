using VotingApp.Models;

namespace VotingApp.ViewModel
{
    public class MultipleChoiceVM
    {
        public CreatedVote createdVote { get; set; }
        public VoteOption votingOptions { get; set; }

        public string voteOption { get; set; }
    }
}
