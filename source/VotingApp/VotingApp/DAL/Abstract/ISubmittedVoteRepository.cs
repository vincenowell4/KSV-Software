using System;
using System.Collections.Generic;
using VotingApp.Models;

namespace VotingApp.DAL.Abstract
{
    public interface ISubmittedVoteRepository
    {
        public Dictionary<VoteOption, SubmittedVote> GetAllSubmittedVotesWithLoggedInUsers(int id, IList<VoteOption> options);
        public Dictionary<VoteOption, int> GetAllSubmittedVotesForUsersNotLoggedIn(int id, IList<VoteOption> options);
        public Dictionary<VoteOption, int> TotalVotesForEachOption(int id, IList<VoteOption> options);
        public int GetTotalSubmittedVotes(int id);
        public Dictionary<VoteOption, int> GetWinner(Dictionary<VoteOption, int> submittedVotes);
    }
}
