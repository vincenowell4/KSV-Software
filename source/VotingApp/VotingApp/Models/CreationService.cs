using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VotingApp.DAL.Abstract;
using System.Reflection;

namespace VotingApp.Models
{
    public class CreationService
    {
        
        private readonly ICreatedVoteRepository _createdVoteRepository;
        private readonly IVoteTypeRepository _voteTypeRepository;
        private readonly VoteCreationService _voteCreationService;
        private readonly IVoteOptionRepository _voteOptionRepository;
        private readonly IAppLogRepository _appLogRepository;
        private readonly ITimeZoneRepo _timeZoneRepository;
        public CreationService(
            ICreatedVoteRepository createdVoteRepository,
            IVoteTypeRepository voteTypeRepository,
            VoteCreationService voteCreationService,
            IVoteOptionRepository voteOptionRepository,
            IAppLogRepository appLogRepository,
            ITimeZoneRepo timeZoneRepo
        )
        {
            _createdVoteRepository = createdVoteRepository;
            _voteTypeRepository = voteTypeRepository;
            _voteCreationService = voteCreationService;
            _voteOptionRepository = voteOptionRepository;
            _appLogRepository = appLogRepository;
            _timeZoneRepository = timeZoneRepo;
        }
        public string Create(ref CreatedVote createdVote)
        {
            MethodBase method = MethodBase.GetCurrentMethod();
            if (createdVote.VoteTypeId == 1)
            {
                createdVote.VoteOptions = _voteTypeRepository.CreateYesNoVoteOptions();
            }

            if (createdVote.VoteTypeId == 3 && createdVote.RoundNumber == 0)
            {
                createdVote.RoundNumber = 1;
            }
            if (createdVote.VoteOpenDateTime == null)
                createdVote.VoteOpenDateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, _timeZoneRepository.GetById(createdVote.TimeZoneId).TimeName);

            if (createdVote.VoteCloseDateTime == null)
            {
                createdVote.VoteCloseDateTime = createdVote.VoteOpenDateTime.Value.AddHours(24);
            }
            try
            {
                if(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, _timeZoneRepository.GetById(createdVote.TimeZoneId).TimeName) >= createdVote.VoteOpenDateTime) 
                {
                    createdVote.VoteAccessCode = _voteCreationService.generateCode();
                }
                _createdVoteRepository.AddOrUpdate(createdVote);
            }
            catch (Exception ex)
            {
                _appLogRepository.LogError(method.ReflectedType.Name, method.Name, "There was an error generating a code for this created vote id: " + createdVote.Id + " , Error message: " + ex);
                return ex.Message;
            }

            _appLogRepository.LogInfo(method.ReflectedType.Name, method.Name, "Successfully created vote with id: " + createdVote.Id);
            return "";
        }

        public string AddVoteAccessCode(ref CreatedVote createdVote)
        {
            MethodBase method = MethodBase.GetCurrentMethod();
            try
            {
                if (createdVote.VoteAccessCode == null)
                    createdVote.VoteAccessCode = _voteCreationService.generateCode();

                _createdVoteRepository.AddOrUpdate(createdVote);
            }
            catch (Exception ex)
            {
                _appLogRepository.LogError(method.ReflectedType.Name, method.Name, "There was an error generating a poll access code for created vote id: " + createdVote.Id + ", Error message: " + ex);
                return ex.Message;
            }

            return createdVote.VoteAccessCode;
        }


        public string Edit(ref CreatedVote createdVote, int oldVoteTypeId)
        {
            MethodBase method = MethodBase.GetCurrentMethod();
            try
            {
                //createdVote.VoteAccessCode = _voteCreationService.generateCode();//shouldnt need to regenerate
                createdVote = _createdVoteRepository.AddOrUpdate(createdVote);
            }
            catch (Exception ex)
            {
                _appLogRepository.LogError(method.ReflectedType.Name, method.Name, "Error occurred when trying to edit vote with id: " + createdVote.Id + ", Error message: " + ex);
                return ex.Message;
            }
            createdVote = _createdVoteRepository.GetById(createdVote.Id); //this is for checking what the vote is in the db
            if (createdVote.VoteTypeId == 1) //going from anything to yes/no
            {
                //remove previous options before adding in the new ones
                _voteOptionRepository.RemoveAllOptions(createdVote.VoteOptions.ToList());
                createdVote.VoteOptions = _voteTypeRepository.CreateYesNoVoteOptions();
                //createdVote.VoteAudioBytes = _googleTtsService.CreateVoteAudio(createdVote);
                createdVote = _createdVoteRepository.AddOrUpdate(createdVote);
            }

            if (createdVote.VoteTypeId != 1 && oldVoteTypeId == 1) //going from yes/no to any other type of vote 
            {
                //remove all options
                //var voteOpts = _voteOptionRepository.GetAllByVoteID(createdVote.VoteTypeId);
                _voteOptionRepository.RemoveAllOptions(createdVote.VoteOptions.ToList());
                //createdVote.VoteOptions = new List<VoteOption>();
                //createdVote.VoteAudioBytes = _googleTtsService.CreateVoteAudio(createdVote);
                createdVote = _createdVoteRepository.AddOrUpdate(createdVote);
            }
            _appLogRepository.LogInfo(method.ReflectedType.Name, method.Name, "Successfully edited vote with id: " + createdVote.Id);
            return "";
        }
        public IEnumerable<VoteAuthorizedUser> ParseUserList(int id ,string userString)
        {
            var userList = new List<string>();
            userList = userString.Split(',').ToList();
            userList = userList.Select(user => user.Trim()).ToList();
            List<VoteAuthorizedUser> voteAuthorizedUser = new List<VoteAuthorizedUser>();
            foreach (var user in userList)
            {
                voteAuthorizedUser.Add(new VoteAuthorizedUser { CreatedVoteId = id, UserName = user });
            }
            return voteAuthorizedUser;
        }
        public string AuthorizedUsersToString(List<VoteAuthorizedUser> userList)
        {
            string userString = "";
            foreach(var user in userList)
            {
                if(userList.Count == 1)
                {
                    userString += user.UserName;
                }
                else
                {
                    userString += user.UserName + ",";
                }
                
            }
            if(userString.LastOrDefault() == ',')
            {
                userString = userString.Remove(userString.Length -1, 1);
            }
            return userString;
        }
    }
}
