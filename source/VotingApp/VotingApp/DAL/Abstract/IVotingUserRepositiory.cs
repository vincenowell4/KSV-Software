using VotingApp.Models;

namespace VotingApp.DAL.Abstract
{
    public interface IVotingUserRepositiory
    {
        public void RemoveUser(VotingUser votingUser);
        public VotingUser AddOrUpdate(VotingUser votingUser);

        public VotingUser GetUserByAspId(string id);

        public VotingUser GetUserById(int id);
    }
}
