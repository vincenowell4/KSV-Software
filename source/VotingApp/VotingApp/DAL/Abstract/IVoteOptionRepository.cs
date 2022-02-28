using System;
using System.Collections.Generic;
using VotingApp.Models;

namespace VotingApp.DAL.Abstract
{
    public interface IVoteOptionRepository
    {
        public bool RemoveOptionById(int id);
        public bool RemoveAllOptions(List<VoteOption> voteOptions);

        public VoteOption GetById(int id);
        public IList<VoteOption> GetAllByVoteID(int id);

    }
}
