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

        public SubmittedVote GetByUserIdAndVoteId(int userId, int voteId);
        List<SubmittedVote> GetCastVotesById(int v);
        SubmittedVote EditCastVote(int v1, int v2);

        public IList<int> TotalVotesPerOption(int id, IList<VoteOption> options);
        public IList<string> MatchingOrderOptionsList(int id, IList<VoteOption> options);
    }
}
