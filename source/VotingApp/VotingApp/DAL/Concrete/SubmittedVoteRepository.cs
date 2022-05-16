using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
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

        public Dictionary<string, string> GetAllSubmittedVotesWithLoggedInUsers(int id, IList<VoteOption> options)
        {
            List<SubmittedVote> subVotes = new List<SubmittedVote>();
            Dictionary<string, string> votesWithUsers = new Dictionary<string, string>();

            var createdVote = _context.CreatedVotes.Where(v => v.Id == id).Include(vo => vo.VoteOptions).Include(sv => sv.SubmittedVotes).ToList();

            if (createdVote != null && createdVote[0].SubmittedVotes != null)
            {
                for (int i = 0; i < createdVote[0].SubmittedVotes.Count; i++)
                {
                    if (createdVote[0].SubmittedVotes.ElementAt(i).User != null)
                    {
                        string optString = options.Where(o => o.Id == createdVote[0].SubmittedVotes.ElementAt(i).VoteChoice).FirstOrDefault().VoteOptionString;
                        string usrName = createdVote[0].SubmittedVotes.ElementAt(i).User.UserName;
                        votesWithUsers.Add(usrName, optString);
                    }
                }
            }
            return votesWithUsers;
        }

        public SubmittedVote GetVoteByIp(string ip, int voteId)
        {
            return _context.SubmittedVotes.Where(a => a.Id == voteId && a.UserIp == ip).FirstOrDefault();
        }
        //public Dictionary<string, string> GetAllSubmittedVotesWithLoggedInUsers(int id, IList<VoteOption> options)
        //{
        //    var submittedVotes = _context.SubmittedVotes.Where(a => a.CreatedVoteId == id && a.User != null).ToList();

        //    Dictionary<string, string> votesWithUsers = new Dictionary<string, string>();

        //    foreach (var vote in submittedVotes)
        //    {
        //        foreach (var option in options)
        //        {
        //            if (vote.VoteChoice == option.Id)
        //            {
        //                if (!votesWithUsers.Keys.Contains(option.VoteOptionString))
        //                {
        //                    votesWithUsers.Add(option.VoteOptionString, vote.User.UserName);
        //                }

        //            }
        //        }
        //    }

        //    return votesWithUsers;
        //}


        //public Dictionary<string, string> GetAllSubmittedVotesWithLoggedInUsers(int id, IList<VoteOption> options)
        //{
        //    List<SubmittedVote> subVotes = new List<SubmittedVote>();
        //    Dictionary<string, string> votesWithUsers = new Dictionary<string, string>();

        //    var createdVote = _context.CreatedVotes.Include(vo => vo.VoteOptions).Where(v => v.Id == id).ToList();

        //    if (createdVote != null)
        //        subVotes = createdVote.FirstOrDefault().SubmittedVotes.ToList();

        //    if (subVotes != null)
        //    {
        //        for (int i = 0; i < subVotes.Count; i++)
        //        {
        //            string optString = options.Where(o => o.Id == subVotes[i].VoteChoice).FirstOrDefault().VoteOptionString;
        //            votesWithUsers.Add(subVotes[i].User.UserName, optString);
        //        }
        //    }


        //    return votesWithUsers;
        //}

        public Dictionary<VoteOption, int> GetAllSubmittedVotesForUsersNotLoggedIn(int id, IList<VoteOption> options)
        {
            var submittedVotes = _context.SubmittedVotes.Where(a => a.User == null && a.CreatedVoteId == id).ToList();
            var grouped = submittedVotes.GroupBy(a => a.VoteChoice);
            Dictionary<VoteOption, int> votesWithOutUsers = new Dictionary<VoteOption, int>();

            foreach (var vote in grouped)
            {
                foreach (var option in options)
                {
                    if (vote.Key == option.Id)
                    {
                        votesWithOutUsers.Add(option, vote.Count());
                    }
                }
            }

            var sortedDict = votesWithOutUsers.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            return sortedDict;
        }

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

        public int GetTotalSubmittedVotes(int id)
        {
            return _context.SubmittedVotes.Where(a => a.CreatedVoteId == id).ToList().Count();
        }

        public Dictionary<VoteOption, int> GetWinner(Dictionary<VoteOption, int> submittedVotes)
        {
            if (submittedVotes == null)
            {
                throw new ArgumentNullException(nameof(submittedVotes));
            }

            var max = submittedVotes.Max(a => a.Value);

            Dictionary<VoteOption, int> winners = new Dictionary<VoteOption, int>();

            foreach (var vote in submittedVotes)
            {
                if (vote.Value == max)
                {
                    winners.Add(vote.Key, vote.Value);
                }
            }

            return winners;
        }

        public SubmittedVote GetByUserIdAndVoteId(int userId, int voteId)
        {
            return _context.SubmittedVotes.Where(s => s.UserId == userId && s.CreatedVoteId == voteId).FirstOrDefault();
        }

        public SubmittedVote GetVoteById(int id)
        {
            return _context.SubmittedVotes.Where(s => s.Id == id).FirstOrDefault();
        }

        public List<SubmittedVote> GetCastVotesById(int id)
        {
            return _context.SubmittedVotes.Where(a => a.UserId == id).OrderByDescending(a => a.DateCast).ToList();
        }

        public SubmittedVote EditCastVote(int voteId, int choiceId)
        {
            var vote = _context.SubmittedVotes.Where(a => a.Id == voteId).FirstOrDefault();
            if (vote != null && vote.VoteChoice != choiceId && (vote.CreatedVote.VoteCloseDateTime >= TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, vote.CreatedVote.TimeZone.TimeName) || vote.CreatedVote.VoteCloseDateTime == null) && vote.CreatedVote.VoteOptions.Select(c => c.Id).ToList().Contains(choiceId))
            {
                vote.VoteChoice = choiceId;
                _context.SubmittedVotes.Update(vote);
                _context.SaveChanges();
            }
            return vote;
        }

        public IList<int> TotalVotesPerOption(int id, IList<VoteOption> options)
        {
           var totals = _context.SubmittedVotes.AsEnumerable().Where(a => a.CreatedVoteId == id).GroupBy(g => g.VoteChoice).ToList();
           IList<int> votesList = new List<int>();

           foreach (var vote in totals)
           {
               votesList.Add(vote.Count());
           }

           return votesList;
        }

        public IList<string> MatchingOrderOptionsList(int id, IList<VoteOption> options)
        {
            var totals = _context.SubmittedVotes.AsEnumerable().Where(a => a.CreatedVoteId == id).GroupBy(g => g.VoteChoice).ToList();
            IList<string> optionsList = new List<string>();

            foreach (var total in totals)
            {
                foreach (var option in options)
                {
                    if (option.Id == total.Key)
                    {
                        optionsList.Add(option.VoteOptionString);
                    }
                }
            }

            return optionsList;
        }
    }
}
