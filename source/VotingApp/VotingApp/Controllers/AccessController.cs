using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VotingApp.DAL.Abstract;
using Microsoft.AspNetCore.Identity;
using VotingApp.ViewModel;
using VotingApp.Models;

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

        public AccessController(ILogger<HomeController> logger, 
            ICreatedVoteRepository createdVoteRepo, 
            IVoteTypeRepository voteTypeRepository,
            UserManager<IdentityUser> userManager,
            IVotingUserRepositiory votingUserRepositiory,
            IVoteOptionRepository voteOptionRepository,
            ISubmittedVoteRepository subVoteRepository)
        {
            _logger = logger;
            _createdVoteRepository = createdVoteRepo;
            _voteTypeRepository = voteTypeRepository;
            _userManager = userManager;
            _votingUserRepository = votingUserRepositiory;
            _voteOptionRepository = voteOptionRepository;
            _subVoteRepository = subVoteRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult VoteHistory()
        {
            if(!User.Identity.IsAuthenticated)
            {
                return Redirect("~/Identity/Account/Login");
            }
            VotingUser user = _votingUserRepository.GetUserByAspId(_userManager.GetUserId(User));
            if (user == null)
            {
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
            var subvote = _subVoteRepository.GetVoteById(id);
            var vote = subvote.CreatedVote;
            if (vote != null && (vote.VoteOpenDateTime == null || vote.VoteOpenDateTime <= DateTime.Now) && (vote.VoteCloseDateTime == null || vote.VoteCloseDateTime >= DateTime.Now))
            {
                if(vote.PrivateVote == true)
                {
                    if (!User.Identity.IsAuthenticated)
                    {
                        return Redirect("~/Identity/Account/Login");
                    }
                    if (!vote.VoteAuthorizedUsers.Select(a => a.UserName).ToList().Contains(_userManager.GetUserName(User)))
                    {
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
            var model = new SubmitConfirmationModel {createdVote = item.CreatedVote, OptionId = item.VoteChoice, CreateId = item.Id };
            return RedirectToAction("SubmitConfirmation", model);

        }

        [HttpGet]
        public IActionResult Access(string code)
        {
            SubmitVoteVM model = new SubmitVoteVM();
            model.vote = _createdVoteRepository.GetVoteByAccessCode(code);
            
            if (model.vote != null && model.vote.VoteCloseDateTime >= DateTime.Now)
            {
                if (model.vote.PrivateVote == true)
                {
                    if (!User.Identity.IsAuthenticated)
                    {
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
            else if (model.vote != null && model.vote.VoteCloseDateTime < DateTime.Now)
            {
                ViewBag.ErrorMessage = $"The Voting Window Has Closed\nVoting Closed on {model.vote.VoteCloseDateTime.Value.Month}/{model.vote.VoteCloseDateTime.Value.Day}/{model.vote.VoteCloseDateTime.Value.Year} at {model.vote.VoteCloseDateTime.Value.TimeOfDay}";
                return View("Index");
            }
            else
            {
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
            var subvote = new SubmittedVote{ User = user, CreatedVote = vote, VoteChoice = choice, DateCast=DateTime.Now};
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
