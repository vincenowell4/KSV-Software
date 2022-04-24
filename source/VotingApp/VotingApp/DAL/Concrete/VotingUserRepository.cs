using VotingApp.DAL.Abstract;
using VotingApp.Models;

namespace VotingApp.DAL.Concrete
{
    public class VotingUserRepository : IVotingUserRepositiory
    {
        private VotingAppDbContext _context;

        public VotingUserRepository(VotingAppDbContext ctx)
        {
            _context = ctx;
        }
        public VotingUser AddOrUpdate(VotingUser votingUser)
        {
            if (votingUser == null)
            {
                throw new ArgumentNullException("Entity must not be null to add or update");
            }
            _context.VotingUsers.Update(votingUser);
            _context.SaveChanges();
            return votingUser;
        }

        public void RemoveUser(VotingUser votingUser)
        {
            if (votingUser == null)
            {
                throw new ArgumentNullException("Entity must not be null to delete");
            }
            _context.VotingUsers.Remove(votingUser);
            _context.SaveChanges();
            
        }
        public VotingUser GetUserByAspId(string id)
        {
            return _context.VotingUsers.Where(a => a.NetUserId == id).FirstOrDefault();
        }

        public VotingUser GetUserById(int id)
        {
            return _context.VotingUsers.Where(a => a.Id == id).FirstOrDefault();
        }

    }
}
