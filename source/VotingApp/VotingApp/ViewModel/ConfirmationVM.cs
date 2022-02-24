using System.ComponentModel.DataAnnotations;

namespace VotingApp.ViewModel
{
    public class ConfirmationVM
    {
        public string VoteTitle { get; set; }
        public string VoteDescription { get; set; }
        public string VoteType { get; set; }
        public string ChosenVoteDescriptionHeader { get; set; }
        public List<string> VotingOptions { get; set; }
        public int ID { get; set; }
        public string VoteAccessCode { get; set; }
    }
}
