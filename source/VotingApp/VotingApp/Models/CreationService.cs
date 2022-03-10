using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VotingApp.DAL.Abstract;

namespace VotingApp.Models
{
    public class CreationService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IVotingUserRepositiory _votingUserRepository;
        private readonly ICreatedVoteRepository _createdVoteRepository;
        private readonly IVoteTypeRepository _voteTypeRepository;
        private readonly VoteCreationService _voteCreationService;
        private readonly IVoteOptionRepository _voteOptionRepository;
        public CreationService(
            ICreatedVoteRepository createdVoteRepository,
            IVotingUserRepositiory votingUserRepositiory,
            UserManager<IdentityUser> userManager,
            IVoteTypeRepository voteTypeRepository,
            VoteCreationService voteCreationService,
            IVoteOptionRepository voteOptionRepository)
        {
            _userManager = userManager;
            _votingUserRepository = votingUserRepositiory;
            _createdVoteRepository = createdVoteRepository;
            _voteTypeRepository = voteTypeRepository;
            _voteCreationService = voteCreationService;
            _voteOptionRepository = voteOptionRepository;
        }
        public string Create(ref CreatedVote createdVote)
        {
            if (createdVote.VoteTypeId == 1)
            {
                createdVote.VoteOptions = _voteTypeRepository.CreateYesNoVoteOptions();
            }

            if (createdVote.VoteCloseDateTime == null)
            {
                createdVote.VoteCloseDateTime = DateTime.Now.AddHours(24);
            }
            try
            {
                createdVote.VoteAccessCode = _voteCreationService.generateCode();
                _createdVoteRepository.AddOrUpdate(createdVote);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "";
        }

        public string Edit(ref CreatedVote createdVote, int oldVoteTypeId)
        {
            try
            {
                //createdVote.VoteAccessCode = _voteCreationService.generateCode();//shouldnt need to regenerate
                createdVote = _createdVoteRepository.AddOrUpdate(createdVote);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            createdVote = _createdVoteRepository.GetById(createdVote.Id); //this is for checking what the vote is in the db
            if (createdVote.VoteTypeId == 1) //going from anything to yes/no
            {
                //remove previous options before adding in the new ones
                _voteOptionRepository.RemoveAllOptions(createdVote.VoteOptions.ToList());
                createdVote.VoteOptions = _voteTypeRepository.CreateYesNoVoteOptions();
                createdVote = _createdVoteRepository.AddOrUpdate(createdVote);
            }

            if (createdVote.VoteTypeId != 1 && oldVoteTypeId == 1) //going from yes/no to any other type of vote 
            {
                //remove all options
                _voteOptionRepository.RemoveAllOptions(createdVote.VoteOptions.ToList());
                createdVote = _createdVoteRepository.AddOrUpdate(createdVote);
            }

            return "";
        }
    }
}
