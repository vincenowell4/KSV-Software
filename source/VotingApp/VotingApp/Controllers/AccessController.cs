using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VotingApp.DAL.Abstract;
using Microsoft.AspNetCore.Identity;
using VotingApp.ViewModel;
using VotingApp.Models;
using System.Reflection;

namespace VotingApp.Controllers
{
    public class AccessController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICreatedVoteRepository _createdVoteRepository;
        private readonly IVoteTypeRepository _voteTypeRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IVotingUserRepositiory _votingUserRepository;
        private readonly IVoteOptionRepository _voteOptionRepository;
        private readonly ISubmittedVoteRepository _subVoteRepository;
        private readonly IAppLogRepository _appLogRepository;

        public AccessController(ILogger<HomeController> logger, 
            ICreatedVoteRepository createdVoteRepo, 
            IVoteTypeRepository voteTypeRepository,
            UserManager<IdentityUser> userManager,
            IVotingUserRepositiory votingUserRepositiory,
            IVoteOptionRepository voteOptionRepository,
            ISubmittedVoteRepository subVoteRepository,
            IAppLogRepository appLogRepository)
        {
            _logger = logger;
            _createdVoteRepository = createdVoteRepo;
            _voteTypeRepository = voteTypeRepository;
            _userManager = userManager;
            _votingUserRepository = votingUserRepositiory;
            _voteOptionRepository = voteOptionRepository;
            _subVoteRepository = subVoteRepository;
            _appLogRepository = appLogRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Results(string code)
        {
            MethodBase method = MethodBase.GetCurrentMethod();
            VoteResultsVM vm = new VoteResultsVM();
            vm.GetVoteByAccessCode = _createdVoteRepository.GetVoteByAccessCode(code);

            if (vm.GetVoteByAccessCode == null)
            {
                _appLogRepository.LogError(method.ReflectedType.Name, method.Name, "User entered invalid access code: " + code);
                ViewBag.ErrorMessage = "Invalid Access Code";
                return View("Index");
            }

            if (vm.GetVoteByAccessCode.PrivateVote == true)
            {
                if (!User.Identity.IsAuthenticated)
                {
                    _appLogRepository.LogInfo(method.ReflectedType.Name, method.Name,
                        "Redirected unlogged in user trying to access private vote to /Identity/Account/Login");
                    return Redirect("~/Identity/Account/Login");
                }

                var user = _userManager.GetUserName(User); //current users email 
                if (vm.GetVoteByAccessCode.VoteAuthorizedUsers.Select(a => a.UserName).ToList()
                    .Contains(user)) //check they are in the list
                {
                    vm.VoteTitle = vm.GetVoteByAccessCode.VoteTitle;
                    vm.VoteDescription = vm.GetVoteByAccessCode.VoteDiscription;
                    vm.VoteOptions = _voteOptionRepository.GetAllByVoteID(vm.GetVoteByAccessCode.Id);
                    vm.TotalVotesForEachOption =
                        _subVoteRepository.TotalVotesForEachOption(vm.GetVoteByAccessCode.Id, vm.VoteOptions);
                    vm.TotalVotesCount = _subVoteRepository.GetTotalSubmittedVotes(vm.GetVoteByAccessCode.Id);
                    vm.Winners = _subVoteRepository.GetWinner(vm.TotalVotesForEachOption);

                    return View(vm);
                }
                else
                {
                    _appLogRepository.LogError(method.ReflectedType.Name, method.Name,
                        "Someone tried accessing private vote who is not part of authorized users list for vote id: " + vm.VoteId);
                    ViewBag.ErrorMessage = $"You are not authorized to view poll results.";
                    return View("Index");
                }
            }

            vm.VoteTitle = vm.GetVoteByAccessCode.VoteTitle;
            vm.VoteDescription = vm.GetVoteByAccessCode.VoteDiscription;
            vm.VoteOptions = _voteOptionRepository.GetAllByVoteID(vm.GetVoteByAccessCode.Id);
            vm.TotalVotesForEachOption =
                _subVoteRepository.TotalVotesForEachOption(vm.GetVoteByAccessCode.Id, vm.VoteOptions);
            vm.TotalVotesCount = _subVoteRepository.GetTotalSubmittedVotes(vm.GetVoteByAccessCode.Id);
            vm.Winners = _subVoteRepository.GetWinner(vm.TotalVotesForEachOption);

            return View(vm);
        }

        public IActionResult VoteHistory()
        {
            MethodBase method = MethodBase.GetCurrentMethod();
            if (!User.Identity.IsAuthenticated)
            {
                _appLogRepository.LogInfo(method.ReflectedType.Name, method.Name,
                    "Redirected unlogged in user trying to access vote history to /Identity/Account/Login");
                return Redirect("~/Identity/Account/Login");
            }
            VotingUser user = _votingUserRepository.GetUserByAspId(_userManager.GetUserId(User));
            if (user == null)
            {
                _appLogRepository.LogError(method.ReflectedType.Name, method.Name, "unable to find user in voting user table for vote history page");
                var newUser = new VotingUser { NetUserId = _userManager.GetUserId(User), UserName = _userManager.GetUserName(User) };
                user = _votingUserRepository.AddOrUpdate(newUser);
            }
            
            var voteList = _subVoteRepository.GetCastVotesById(user.Id);
            List<SubmittedVoteHistoryVM> voteListVM = new List<SubmittedVoteHistoryVM>();
            foreach(var vote in voteList)
            {
                SubmittedVoteHistoryVM submittedVoteHistoryVM = new SubmittedVoteHistoryVM();
                submittedVoteHistoryVM.subVote = vote;
                submittedVoteHistoryVM.voteOption = vote.CreatedVote.VoteOptions.Where(a => a.Id == vote.VoteChoice).FirstOrDefault();
                voteListVM.Add(submittedVoteHistoryVM);
            } 
            return View(voteListVM);

        }

        public IActionResult EditSubVote(int id)
        {
            MethodBase method = MethodBase.GetCurrentMethod();
            var subvote = _subVoteRepository.GetVoteById(id);
            var vote = subvote.CreatedVote;
            if (vote != null && (vote.VoteOpenDateTime == null || vote.VoteOpenDateTime <= TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, vote.TimeZone.TimeName) && (vote.VoteCloseDateTime == null || vote.VoteCloseDateTime >= TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, vote.TimeZone.TimeName))))
            {
                if(vote.PrivateVote == true)
                {
                    if (!User.Identity.IsAuthenticated)
                    {
                        _appLogRepository.LogError(method.ReflectedType.Name, method.Name, 
                            "redirected unlogged in user from editing vote for a private vote to /Identity/Account/Login");
                        return Redirect("~/Identity/Account/Login");
                    }
                    if (!vote.VoteAuthorizedUsers.Select(a => a.UserName).ToList().Contains(_userManager.GetUserName(User)))
                    {
                        _appLogRepository.LogError(method.ReflectedType.Name, method.Name, 
                            "unauthorized user tried to access private vote with id: " + vote.Id);
                        ViewBag.ErrorMessage = $"You are not authorized for this vote.";
                        return View("Index");
                    }
                    return View("SubmitVote", new SubmitVoteVM { vote = vote, options = vote.VoteOptions.ToList(), submittedVote = subvote });
                }
            }
            return View("SubmitVote", new SubmitVoteVM { vote = vote, options = vote.VoteOptions.ToList(), submittedVote = subvote });
        }
        public IActionResult EditCastVote(int Id, int choice)
        {
            var item = _subVoteRepository.EditCastVote(Id, choice);
            var model = new SubmitConfirmationModel { OptionId = item.VoteChoice, CreateId = item.CreatedVoteId };
            return RedirectToAction("SubmitConfirmation", model);

        }

        [HttpGet]
        public IActionResult Access(string code)
        {
            MethodBase method = MethodBase.GetCurrentMethod();
            SubmitVoteVM model = new SubmitVoteVM();
            model.vote = _createdVoteRepository.GetVoteByAccessCode(code);
            
            if (model.vote != null && model.vote.VoteCloseDateTime >= TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, model.vote.TimeZone.TimeName))
            {
                if (model.vote.PrivateVote == true)
                {
                    if (!User.Identity.IsAuthenticated)
                    {
                        _appLogRepository.LogError(method.ReflectedType.Name, method.Name,
                            "redirected unlogged in user trying to access private vote to /Identity/Account/Login");
                        return Redirect("~/Identity/Account/Login");
                    }
                    else
                    {
                        VotingUser user = _votingUserRepository.GetUserByAspId(_userManager.GetUserId(User));
                        if (user == null)
                        {
                            var newUser = new VotingUser { NetUserId = _userManager.GetUserId(User), UserName = _userManager.GetUserName(User) };
                            user = _votingUserRepository.AddOrUpdate(newUser);
                        }
                        foreach (var users in model.vote.VoteAuthorizedUsers)
                        {
                            if(users.UserName == user.UserName)
                            {
                                if (user != null && user.Id != 0)
                                {
                                    SubmittedVote subVote = _subVoteRepository.GetByUserIdAndVoteId(user.Id, model.vote.Id);
                                    if (subVote != null)
                                    {
                                        model.submittedVote = subVote; // user already submitted a vote - store it in View Model
                                    }
                                }
                                return View("SubmitVote", model);
                            }
                        }

                        _appLogRepository.LogError(method.ReflectedType.Name, method.Name, 
                            "Unauthorized user tried accessing private vote with id: " + model.vote.Id);
                        ViewBag.ErrorMessage = $"You are not authorized for this vote.";
                        return View("Index");
                    }
                }
                if (User.Identity.IsAuthenticated) //if user is logged in, check to see if they've already submitted a vote
                {
                    VotingUser user = _votingUserRepository.GetUserByAspId(_userManager.GetUserId(User));
                    if (user != null && user.Id != 0)
                    {
                        SubmittedVote subVote = _subVoteRepository.GetByUserIdAndVoteId(user.Id, model.vote.Id);
                        if (subVote != null)
                        {
                            model.submittedVote = subVote; // user already submitted a vote - store it in View Model
                        }
                    }
                }
                return View("SubmitVote", model);
            }
            else if (model.vote != null && model.vote.VoteCloseDateTime < TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, model.vote.TimeZone.TimeName))
            {
                ViewBag.ErrorMessage = $"The Voting Window Has Closed\nVoting Closed on {model.vote.VoteCloseDateTime.Value.Month}/{model.vote.VoteCloseDateTime.Value.Day}/{model.vote.VoteCloseDateTime.Value.Year} at {model.vote.VoteCloseDateTime.Value.TimeOfDay}";
                return View("Index");
            }
            else
            {
                _appLogRepository.LogError(method.ReflectedType.Name, method.Name, "Invalid access code on access page: " + code);
                ViewBag.ErrorMessage = "Invalid Access Code";
                return View("Index");
            }
        }

        [HttpGet]
        public IActionResult CastVote(int voteID, int choice)
        {
            
            var vote = _createdVoteRepository.GetById(voteID);
            var user = new VotingUser();
            if (choice == 0 || choice == null)
            {
                SubmitVoteVM modelVM = new SubmitVoteVM();
                modelVM.vote = vote;
                ViewBag.ErrorMessage = "Please make a selection.";
                return View("SubmitVote", modelVM);
            }
            if (User.Identity.IsAuthenticated)
            {
                user = _votingUserRepository.GetUserByAspId(_userManager.GetUserId(User));
                if (user == null)
                {
                    var newUser = new VotingUser { NetUserId = _userManager.GetUserId(User), UserName = _userManager.GetUserName(User) };
                    user = _votingUserRepository.AddOrUpdate(newUser);
                }
            }
            else
            {
                user = null;
            }
            var subvote = new SubmittedVote{ User = user, CreatedVote = vote, VoteChoice = choice, DateCast= TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, vote.TimeZone.TimeName) };
            vote.SubmittedVotes.Add(subvote);
            _createdVoteRepository.AddOrUpdate(vote);
            var model = new SubmitConfirmationModel {OptionId=subvote.VoteChoice, CreateId=vote.Id};
            return RedirectToAction("SubmitConfirmation", model);
        }

        public IActionResult SubmitConfirmation(SubmitConfirmationModel model)
        {
            model.votingOption = _voteOptionRepository.GetById(model.OptionId);
            model.createdVote = _createdVoteRepository.GetById(model.CreateId);
            return View(model);
        }
    }
}
