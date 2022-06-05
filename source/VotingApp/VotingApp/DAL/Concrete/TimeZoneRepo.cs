using VotingApp.DAL.Abstract;
using VotingApp.Models;

namespace VotingApp.DAL.Concrete
{
    public class TimeZoneRepo : ITimeZoneRepo
    {
        private VotingAppDbContext _context;

        public TimeZoneRepo(VotingAppDbContext ctx)
        {
            _context = ctx;
        }
        public List<VoteTimeZone> GetAllTimeZones()
        {
            return _context.VoteTimeZones.ToList();
        }

        public VoteTimeZone GetById(int id)
        {
            return _context.VoteTimeZones.Where(a => a.Id == id).FirstOrDefault();
        }
    }
}
