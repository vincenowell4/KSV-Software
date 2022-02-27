using System;
using System.Collections.Generic;
using VotingApp.DAL.Abstract;
using VotingApp.Models;

namespace VotingApp.DAL.Concrete
{
    public class VoteOptionRepository : IVoteOptionRepository
    {
        private VotingAppDbContext _context;

        public VoteOptionRepository(VotingAppDbContext ctx)
        {
            _context = ctx;
        }
        public VoteOption GetById(int id)
        {
            return _context.VoteOptions.Where(a => a.Id == id).FirstOrDefault();
        }

        public IList<VoteOption> GetAllByVoteID(int id)
        {
            return _context.VoteOptions.Where(a => a.CreatedVoteId == id).ToList();
        }
    }
}
