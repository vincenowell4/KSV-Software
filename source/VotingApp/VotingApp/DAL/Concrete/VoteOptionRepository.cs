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
            if (id == 0)
            {
                throw new ArgumentException();
            }

            var remove = _context.VoteOptions.Where(a => a.Id == id).FirstOrDefault();
            _context.Remove(remove);
            _context.SaveChanges();
            return true;
        }

        public void RemoveAllOptions(List<VoteOption> voteOptions)
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
        }
    }
}
