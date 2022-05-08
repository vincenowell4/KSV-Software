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
        public IList<CreatedVote> GetAllClosedMultiRoundVotes();

        public void SendEmails(IList<VoteAuthorizedUser> users, CreatedVote vote, string accessCode);

        public IList<CreatedVote> GetOpenCreatedVotes(IList<CreatedVote> createdVotes);
        public IList<CreatedVote> GetClosedCreatedVotes(IList<CreatedVote> createdVotes);

        public string GetMultiRoundVoteDuration(int id);
    }
}
