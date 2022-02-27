using System;
using System.Collections.Generic;
using VotingApp.Models;

namespace VotingApp.DAL.Abstract
{
    public interface IVoteOptionRepository
    {
        public VoteOption GetById(int id);
        public IList<VoteOption> GetAllByVoteID(int id);
    }
}
