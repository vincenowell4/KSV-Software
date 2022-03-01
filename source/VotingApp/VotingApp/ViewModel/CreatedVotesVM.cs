using System.ComponentModel.DataAnnotations;
using System.Linq;
using VotingApp.Models;
using VotingApp.DAL.Abstract;

namespace VotingApp.ViewModel
{
    public class CreatedVotesVM
    {
        [StringLength(350)]
        public string VoteTitle { get; set; }
        [StringLength(1000)]
        public string VoteDescription { get; set; }
        public int Id { get; set; }
        public int UserId { get; set; }
        public IList<CreatedVote> CreatedVotes { get; set; }
        private readonly ICreatedVoteRepository _cvRepository;

        public CreatedVotesVM(int userId, ICreatedVoteRepository cvRepo)
        {
            UserId = userId;
            _cvRepository = cvRepo;
        }

        public void GetCreatedVotesListForUserId(int userId)
        {
            CreatedVotes = _cvRepository.GetAllForUserId(userId);
            return;
        }
    }
}


