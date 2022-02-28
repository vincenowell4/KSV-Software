using VotingApp.Models;
namespace VotingApp.ViewModel
{
    public class SubmitConfirmationModel
    {
        public int OptionId { get; set; }
        public int CreateId { get; set; }
        public CreatedVote createdVote { get; set; }
        public VoteOption votingOption { get; set; }
    }
}
