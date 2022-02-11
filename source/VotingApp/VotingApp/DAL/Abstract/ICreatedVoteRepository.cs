using System;
using System.Collections.Generic;
using VotingApp.Models;

namespace VotingApp.DAL.Abstract
{
    public interface ICreatedVoteRepository
    {
        public CreatedVote AddOrUpdate(CreatedVote createdVote);
        public Boolean SetAnonymous(int id); 
        public string GetVoteDescription(int id);
    }
}
