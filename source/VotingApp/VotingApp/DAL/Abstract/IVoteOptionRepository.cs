using VotingApp.Models;

namespace VotingApp.DAL.Abstract
{
    public interface IVoteOptionRepository
    {
        public bool RemoveOptionById(int id);
        public void RemoveAllOptions(List<VoteOption> voteOptions);
    }
}
