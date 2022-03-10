using Microsoft.CodeAnalysis.CSharp.Syntax;
using VotingApp.DAL.Abstract;
using VotingApp.Models;

namespace VotingApp.DAL.Concrete
{
    public class SubmittedVoteRepository : ISubmittedVoteRepository
    {
        private VotingAppDbContext _context;

        public SubmittedVoteRepository(VotingAppDbContext ctx)
        {
            _context = ctx;
        }
        //public List<SubmittedVote> GetAllSubmittedVotes(int id)
        //{
        //    var submittedVotes = _context.SubmittedVotes.Where(a => a.CreatedVoteId == id).ToList();
        //    if (submittedVotes == null)
        //    {
        //        throw new NullReferenceException(); 
        //    }
        //    return submittedVotes;
        //}

        public Dictionary<VoteOption, int> TotalVotesForEachOption(int id, IList<VoteOption> options)
        {
            if (options == null)
            {
                throw new NullReferenceException();
            }

            var submittedVotes = _context.SubmittedVotes.AsEnumerable().Where(a => a.CreatedVoteId == id).GroupBy(g => g.VoteChoice).ToList();

            Dictionary<VoteOption, int> votesWithTotal = new Dictionary<VoteOption, int>();
            foreach (var option in options)
            {
                votesWithTotal.Add(option, 0);
            }

            foreach (var votes in submittedVotes) //loop through submitted votes and update count on options that were voted on 
            {
                foreach (var vote in votesWithTotal)
                {
                    if (votes.Key == vote.Key.Id)
                    {
                        votesWithTotal[vote.Key] = votes.Count();
                    }
                }

            }
            var sortedDict = votesWithTotal.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            return sortedDict;
        }

        public SubmittedVote GetByUserIdAndVoteId(int userId, int voteId)
        {
            return _context.SubmittedVotes.Where(s => s.UserId == userId && s.CreatedVoteId == voteId).FirstOrDefault();
        }
    }
}
