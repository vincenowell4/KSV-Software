using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using VotingApp.DAL.Abstract;
using VotingApp.DAL.Concrete;
using VotingApp.Models;

namespace VotingApp.DAL.Concrete
{
    public class VoteTypeRepository : IVoteTypeRepository
    {
        private VotingAppDbContext _context;

        public VoteTypeRepository(VotingAppDbContext ctx)
        {
            _context = ctx;
        }

        public List<VoteType> VoteTypes()
        {
            return _context.VoteTypes.ToList();
        }

        public string GetVoteType(int voteTypeId)
        {
            var votes = _context.VoteTypes.Where(a => a.Id == voteTypeId).SingleOrDefault();
            if (votes == null)
            {
                return null;
            }

            string voteType = votes.Type;
            
           
            return voteType;
        }

        public string GetChosenVoteHeader(string voteType)
        {
            if (voteType == "Yes/No Vote")
            {
                return "You have chosen to create a yes/no vote"; 
            }
            return null;
        }

        public List<string> GetVoteOptions(string voteType)
        {
            List<string> voteOptions = new List<string>();
            
            if (voteType == "Yes/No Vote")
            {
                voteOptions.Add("Yes");
                voteOptions.Add("No");
            }
            return voteOptions;
        }
    }
}
