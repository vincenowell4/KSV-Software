using VotingApp.Models;

namespace VotingApp.DAL.Abstract
{
    public interface IVoteOptionRepository
    {
        public void RemoveOptionById(int id);
        public void RemoveAllOptions(List<VoteOption> voteOptions);
    }
}
