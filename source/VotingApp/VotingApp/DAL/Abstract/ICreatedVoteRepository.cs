using System;
using System.Collections.Generic;
using VotingApp.Models;

namespace VotingApp.DAL.Abstract
{
    public interface ICreatedVoteRepository
    {
        public CreatedVote GetById(int id);
        public CreatedVote AddOrUpdate(CreatedVote createdVote);
        public Boolean SetAnonymous(int id);
        public string GetVoteTitle(int id);
        public string GetVoteDescription(int id);
        public CreatedVote GetVoteByAccessCode(string code);
        public IList<CreatedVote> GetAll();
        public IList<CreatedVote> GetAllForUserId(int userId);
        public IList<CreatedVote> GetAllVotesWithNoAccessCode();
    }
}
