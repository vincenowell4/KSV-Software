namespace VotingApp.Models
{
    public class VoteCreationService
    {
        private VotingAppDbContext _context;

        public VoteCreationService(VotingAppDbContext ctx)
        {
            _context = ctx;
        }
        public string generateCode()
        {
            CreatedVote found;
            string code;
            do
            {
                code = Guid.NewGuid().ToString();
                code = code.Take(6).;
                found = _context.CreatedVotes.Where(a => a.VoteAccessCode == code).FirstOrDefault();
            }
            while (found != null);

            return code;
        }
    }
}
