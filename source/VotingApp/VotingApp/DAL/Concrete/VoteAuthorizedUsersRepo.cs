using VotingApp.DAL.Abstract;
using VotingApp.Models;

namespace VotingApp.DAL.Concrete
{
    public class VoteAuthorizedUsersRepo : IVoteAuthorizedUsersRepo
    {
        private VotingAppDbContext _context;

        public VoteAuthorizedUsersRepo(VotingAppDbContext ctx)
        {
            _context = ctx;
        }
        public IList<VoteAuthorizedUser> GetAllUsersByVoteID(int id)
        {
            return _context.VoteAuthorizedUsers.Where(x => x.CreatedVoteId == id).ToList();
        }
        public void RemoveAllByVoteID(int id)
        {
            _context.VoteAuthorizedUsers.RemoveRange(GetAllUsersByVoteID(id));
            _context.SaveChanges();
        }
    }
}
