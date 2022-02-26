using System;
using System.Collections.Generic;
using VotingApp.Models;

namespace VotingApp.DAL.Abstract
{
    public interface IVoteTypeRepository
    {
        public List<VoteType> VoteTypes();
        public string GetVoteType(int voteTypeId); 
        public string GetChosenVoteHeader(string voteType);
        public List<string> GetVoteOptions(string voteType);
        public List<VoteOption> CreateVoteOptions();

        public int CheckForChangeFromYesNoVoteType(int createdVoteId);
    }
}
