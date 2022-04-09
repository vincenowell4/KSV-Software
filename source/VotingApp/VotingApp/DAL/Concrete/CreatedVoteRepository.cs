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
    public class CreatedVoteRepository : ICreatedVoteRepository
    {
        private VotingAppDbContext _context;

        public CreatedVoteRepository(VotingAppDbContext ctx)
        {
            _context = ctx;
        }
        public CreatedVote AddOrUpdate(CreatedVote createdVote)
        {
            if (createdVote == null)
            {
                throw new ArgumentNullException("Entity must not be null to add or update");
            }
            _context.Update(createdVote);
            _context.SaveChanges();
            return createdVote;
        }

        public Boolean SetAnonymous(int id)
        {
            var createdVote = _context.CreatedVotes.Where(a => a.Id == id).FirstOrDefault();

            if (createdVote != null && createdVote.AnonymousVote == false)
            {
                createdVote.AnonymousVote = true;
                AddOrUpdate(createdVote);
                return true;
            }

            return false;
        }

        public CreatedVote GetVoteByAccessCode(string code)
        {
            var vote = _context.CreatedVotes.Where(a => a.VoteAccessCode == code).Include(a => a.VoteType).Include(a => a.VoteOptions).FirstOrDefault();
            return vote;
        }

        public CreatedVote GetById(int id)
        {
            return _context.CreatedVotes.Where(a => a.Id == id).Include(a => a.VoteOptions).FirstOrDefault();
        }

        public string GetVoteTitle(int id)
        {
            var voteTitle = _context.CreatedVotes.Where(a => a.Id == id).Select(ab => ab.VoteTitle).FirstOrDefault();
            if (voteTitle == null)
            {
                return null;
            }
            return voteTitle;
        }
        public string GetVoteDescription(int id)
        {
            var voteDescription = _context.CreatedVotes.Where(a => a.Id == id).Select(ab => ab.VoteDiscription).FirstOrDefault();
            if (voteDescription == null)
            {
                return null;
            }
            return voteDescription;
        }

        public IList<CreatedVote> GetAll()
        {
            return _context.CreatedVotes.Include(a => a.VoteType).ToList();
        }

        public IList<CreatedVote> GetAllForUserId(int userId)
        {
            return _context.CreatedVotes.Include(a => a.VoteType).Where(v => v.UserId == userId).ToList();
        }

        public IList<CreatedVote> GetAllVotesWithNoAccessCode()
        {
            return _context.CreatedVotes.Where(v => v.VoteAccessCode == null).ToList();
        }

    }
}

