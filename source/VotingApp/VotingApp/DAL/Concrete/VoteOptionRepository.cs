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

        public bool RemoveOptionById(int id)
        {
            var remove = _context.VoteOptions.Where(a => a.Id == id).FirstOrDefault();
            _context.Remove(remove);
            _context.SaveChanges();
            return true;
        }

        public bool RemoveAllOptions(List<VoteOption> voteOptions)
        {
            if (voteOptions == null)
            {
                throw new ArgumentNullException();
            }

            foreach (var option in voteOptions)
            {
                _context.Remove(option);
            }

            //get a list of options and do a foreach loop where you remove that option each time 
            _context.SaveChanges();
            return true;
        }

        public VoteOption GetById(int id)
        {
            return _context.VoteOptions.Where(a => a.Id == id).FirstOrDefault();
        }

        public IList<VoteOption> GetAllByVoteID(int id)
        {
            return _context.VoteOptions.Where(a => a.CreatedVoteId == id).ToList();
        }

        public IList<string> GetVoteOptionString(int id)
        {
            return _context.VoteOptions.Where(a => a.CreatedVoteId == id).Select(a => a.VoteOptionString).ToList();
        }
    }
}
