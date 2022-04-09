using VotingApp.Models;

namespace VotingApp.DAL.Abstract
{
    public interface IVoteAuthorizedUsersRepo
    {
        public void RemoveAllByVoteID(int id);
        public IList<VoteAuthorizedUser> GetAllUsersByVoteID(int id);


    }
}
