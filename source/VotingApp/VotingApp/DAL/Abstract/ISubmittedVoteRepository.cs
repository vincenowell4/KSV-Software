using System;
using System.Collections.Generic;
using VotingApp.Models;

namespace VotingApp.DAL.Abstract
{
    public interface ISubmittedVoteRepository
    {
        public Dictionary<VoteOption, int> TotalVotesForEachOption(int id, IList<VoteOption> options);
    }
}
