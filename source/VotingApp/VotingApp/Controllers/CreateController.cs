using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using VotingApp.DAL.Abstract;
using VotingApp.Models;
using VotingApp.ViewModel;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http;

namespace VotingApp.Controllers
{
    public class CreateController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICreatedVoteRepository _createdVoteRepository;
        private readonly IVoteTypeRepository _voteTypeRepository;
        private readonly VoteCreationService _voteCreationService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IVotingUserRepositiory _votingUserRepository;
        private readonly IVoteOptionRepository _voteOptionRepository;
        private readonly CreationService _creationService;
        private readonly ISubmittedVoteRepository _submittedVoteRepository;
        private readonly IVoteAuthorizedUsersRepo _voteAuthorizedUsersRepo;

        public CreateController(ILogger<HomeController> logger, 
            ICreatedVoteRepository createdVoteRepo, 
            IVoteTypeRepository voteTypeRepository, 
            VoteCreationService voteCreationService, 
            UserManager<IdentityUser> userManager,
            IVotingUserRepositiory votingUserRepositiory,
            IVoteOptionRepository voteOptionRepository,
            CreationService creationService,
            ISubmittedVoteRepository submittedVoteRepository,
            IVoteAuthorizedUsersRepo voteAuthorizedUsersRepo)
        {
            _logger = logger;
            _createdVoteRepository = createdVoteRepo;
            _voteTypeRepository = voteTypeRepository;
            _voteCreationService = voteCreationService;
            _userManager = userManager;
            _votingUserRepository = votingUserRepositiory;
            _voteOptionRepository = voteOptionRepository;
            _creationService = creationService;
            _submittedVoteRepository = submittedVoteRepository;
            _voteAuthorizedUsersRepo = voteAuthorizedUsersRepo;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var selectListVoteType = new SelectList(
                _voteTypeRepository.VoteTypes().Select(a => new { Text = $"{a.VotingType}", Value = a.Id }),
                "Value", "Text");
            ViewData["VoteTypeId"] = selectListVoteType;

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index([Bind("VoteTypeId,VoteTitle,VoteDiscription,AnonymousVote,VoteOpenDateTime,VoteCloseDateTime, PrivateVote")]CreatedVote createdVote)
        {
            ModelState.Remove("VoteType");
            ModelState.Remove("VoteAccessCode");
            if (User.Identity.IsAuthenticated != false)
            {
                var vUser = _votingUserRepository.GetUserByAspId(_userManager.GetUserId(User));
                if (vUser == null)
                {
                    var newUser = new VotingUser { NetUserId = _userManager.GetUserId(User), UserName = _userManager.GetUserName(User) };
                    vUser = _votingUserRepository.AddOrUpdate(newUser);
                }
                createdVote.User = vUser;
            }
            if (ModelState.IsValid)
            { 
                var result = _creationService.Create(ref createdVote);
                if (result != "")
                {
                    ViewBag.Message = result;
                    return View(createdVote);
                }
                if(createdVote.PrivateVote == true)
                {
                    createdVote = _createdVoteRepository.GetById(createdVote.Id);
                    AuthorizedUserPageVM vm = new AuthorizedUserPageVM {id = createdVote.Id };
                    return RedirectToAction("AddVoteUsers", vm);
                }
                else if (createdVote.VoteTypeId == 1)
                {
                    return RedirectToAction("Confirmation", createdVote);
                }
                else
                {
                    createdVote = _createdVoteRepository.GetById(createdVote.Id);
                    return View("MultipleChoice", createdVote);
                }
            }
            else
            {
                ViewBag.Message = "An unknown database error occurred while trying to create the item. Please try again.";
                return View(createdVote);
            }
        }

        public IActionResult AddVoteUsers(AuthorizedUserPageVM vm)
        {
            return View("AddVoteUsers",vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult edit([Bind("Id,VoteTypeId,VoteTitle,VoteDiscription,AnonymousVote,VoteOption,VoteOpenDateTime,VoteCloseDateTime,VoteAccessCode, PrivateVote")] CreatedVote createdVote, int oldVoteTypeId)
        {
            ModelState.Remove("VoteType");
            ModelState.Remove("VoteAccessCode");
            if (User.Identity.IsAuthenticated != false)
            {
                createdVote.User = _votingUserRepository.GetUserByAspId(_userManager.GetUserId(User));
            }
            if (ModelState.IsValid)
            {
                
                var result = _creationService.Edit(ref createdVote, oldVoteTypeId);
                if (result != "")
                {
                    ViewBag.Message = result;
                    return View(createdVote);
                }
                createdVote = _createdVoteRepository.GetById(createdVote.Id);
                if (createdVote.PrivateVote == true)
                {
                    
                    var listofUserString = _creationService.AuthorizedUsersToString(createdVote.VoteAuthorizedUsers.ToList());
                    AuthorizedUserPageVM vm = new AuthorizedUserPageVM{ emails = listofUserString, id = createdVote.Id};
                    return RedirectToAction("AddVoteUsers", vm);
                }
                if(createdVote.PrivateVote == false && createdVote.VoteAuthorizedUsers.Count > 0)
                {
                    _voteAuthorizedUsersRepo.RemoveAllByVoteID(createdVote.Id);
                }
                if (createdVote.VoteTypeId == 1)
                {
                    return RedirectToAction("Confirmation", createdVote);
                }
                else
                {
                    createdVote = _createdVoteRepository.GetById(createdVote.Id); 
                    return View("MultipleChoice", createdVote);
                }

            }
            else
            {
                ViewBag.Message = "An unknown database error occurred while trying to create the item. Please try again.";
                return View("Index",createdVote);
            }
        }

        [HttpGet]
        public IActionResult MultipleChoice(CreatedVote createdVote)
        {
            var vote = _createdVoteRepository.GetById(createdVote.Id);
            return View(vote);
        }

        [HttpGet]
        public IActionResult AddMultipleChoiceOption(int id, string option)
        {
            var vote = _createdVoteRepository.GetById(id);
            VoteOption voteOption = new VoteOption();
            voteOption.VoteOptionString = option;
            vote.VoteOptions.Add(voteOption);
            _createdVoteRepository.AddOrUpdate(vote);
            return RedirectToAction("MultipleChoice", vote);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOption()
        {
            return RedirectToAction("MultipleChoice"); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MultipleChoiceToConfirmation(int id)
        {
            var createdVote = _createdVoteRepository.GetById(id);
            return RedirectToAction("Confirmation", createdVote);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveOption(int optionId, int voteId)
        {
            _voteOptionRepository.RemoveOptionById(optionId);
            var vote = _createdVoteRepository.GetById(voteId);
            return RedirectToAction("MultipleChoice", vote);
        }

        [HttpGet]
        public IActionResult Confirmation(CreatedVote createdVote)
        {
            createdVote = _createdVoteRepository.GetById(createdVote.Id);
            var vm = new ConfirmationVM();
            vm.VoteTitle = _createdVoteRepository.GetVoteTitle(createdVote.Id);
            vm.VoteDescription = _createdVoteRepository.GetVoteDescription(createdVote.Id);
            vm.VoteType = _voteTypeRepository.GetVoteType(createdVote.VoteTypeId);
            vm.ChosenVoteDescriptionHeader = _voteTypeRepository.GetChosenVoteHeader(vm.VoteType);
            vm.VotingOptions = createdVote.VoteOptions.ToList();
            vm.ID = createdVote.Id;
            vm.AnonymousVote = createdVote.AnonymousVote;
            vm.VoteAccessCode = createdVote.VoteAccessCode;
            vm.ShareURL =
                $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/Access/{createdVote.VoteAccessCode}";
            vm.VoteCloseDateTime = createdVote.VoteCloseDateTime ?? DateTime.Now;
            if (createdVote.VoteOpenDateTime != null)
                vm.VoteOpenDateTime = createdVote.VoteOpenDateTime;
            vm.VotingAuthorizedUsers = createdVote.VoteAuthorizedUsers.ToList();
            return View(vm);
        }

        [HttpGet]
        public IActionResult CreatedVotesReview()
        {
            int userId = 0;

            if (User.Identity.IsAuthenticated != false)
            {
                VotingUser vUser = _votingUserRepository.GetUserByAspId(_userManager.GetUserId(User));
                if(vUser == null)
                {
                    var newUser = new VotingUser { NetUserId = _userManager.GetUserId(User), UserName = _userManager.GetUserName(User) };
                    vUser = _votingUserRepository.AddOrUpdate(newUser);
                }
                userId = vUser.Id;

                if (userId == 0)
                    return View();

                CreatedVotesVM createdVotesVM = new CreatedVotesVM(userId, _createdVoteRepository);
                createdVotesVM.GetCreatedVotesListForUserId(userId);
                return View(createdVotesVM);
            }
            return View();
        }

        [HttpPost]
        public IActionResult CreatedVotesReview(string voteData)
        {
            if (User.Identity.IsAuthenticated != false && voteData.Length > 0)
            {
                dynamic tmp = JsonConvert.DeserializeObject(voteData);
                int voteId = (int)tmp.voteId;

                CreatedVote voteToEdit = _createdVoteRepository.GetById(voteId);

                voteToEdit.VoteTitle= (string)tmp.voteTitle;
                voteToEdit.VoteDiscription = (string)tmp.voteDesc;

                var voteData2 = new
                {
                    Id = voteId,
                    Title = voteToEdit.VoteTitle,
                    Desc = voteToEdit.VoteDiscription,
                    Url = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/Access/{voteToEdit.VoteAccessCode}"
            };
                JsonResult voteInfo2 = Json(voteData2);

                CreatedVote voteToEdit2 = _createdVoteRepository.AddOrUpdate(voteToEdit);

                return voteInfo2;

            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult VoteAnalyticsButton(int id)
        {
            var createdVote = _createdVoteRepository.GetById(id);
            return RedirectToAction("Analytics", createdVote);
        }

        [HttpGet]
        public IActionResult Analytics(CreatedVote createdVote)
        {
            createdVote = _createdVoteRepository.GetById(createdVote.Id);
            var vm = new AnalyticsVM();
            vm.VoteTitle = createdVote.VoteTitle;
            vm.VoteDescription = createdVote.VoteDiscription;
            vm.VoteOptions = _voteOptionRepository.GetAllByVoteID(createdVote.Id);
            vm.ChartVoteTotals = _submittedVoteRepository.TotalVotesPerOption(createdVote.Id, vm.VoteOptions);
            vm.ChartVoteOptions = _submittedVoteRepository.MatchingOrderOptionsList(createdVote.Id, vm.VoteOptions);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult VoteResultsButton(int id)
        {
            var createdVote = _createdVoteRepository.GetById(id);
            return RedirectToAction("VoteResults", createdVote);
        }

        [HttpGet]
        public IActionResult VoteResults(CreatedVote createdVote)
        {
            createdVote = _createdVoteRepository.GetById(createdVote.Id);
            var vm = new VoteResultsVM();
            vm.VoteTitle = createdVote.VoteTitle;
            vm.VoteDescription = createdVote.VoteDiscription;
            vm.AnonymousVote = createdVote.AnonymousVote;
            vm.VoteId = createdVote.Id;
            vm.VoteOptions = _voteOptionRepository.GetAllByVoteID(createdVote.Id);
            vm.TotalVotesForEachOption = _submittedVoteRepository.TotalVotesForEachOption(createdVote.Id, vm.VoteOptions);
            vm.VotesForLoggedInUsers = _submittedVoteRepository.GetAllSubmittedVotesWithLoggedInUsers(createdVote.Id, vm.VoteOptions);
            vm.VotesForUsersNotLoggedIn = _submittedVoteRepository.GetAllSubmittedVotesForUsersNotLoggedIn(createdVote.Id, vm.VoteOptions);
            vm.TotalVotesCount = _submittedVoteRepository.GetTotalSubmittedVotes(createdVote.Id);
            vm.Winners = _submittedVoteRepository.GetWinner(vm.TotalVotesForEachOption);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Confirmation()
        {
            return RedirectToAction("Index", "Home");
        }

        //[HttpGet] 
        public IActionResult BackToIndexPage(int ID)
        {
            var result = _createdVoteRepository.GetById(ID);
            var selectListVoteType = new SelectList(
                _voteTypeRepository.VoteTypes().Select(a => new { Text = $"{a.VotingType}", Value = a.Id }),
                "Value", "Text");
            ViewData["VoteTypeId"] = selectListVoteType;
            return View("Index",result);
        }
        public IActionResult GetUsers(int id, string userListString)
        {
            var createdVote = _createdVoteRepository.GetById(id);
            if(createdVote.VoteAuthorizedUsers.Count > 0)
            {
                _voteAuthorizedUsersRepo.RemoveAllByVoteID(createdVote.Id);
            }
            if(userListString != null)
            {
                createdVote.VoteAuthorizedUsers = _creationService.ParseUserList(id, userListString).ToList();
                createdVote = _createdVoteRepository.AddOrUpdate(createdVote);
            }

            if (createdVote.VoteTypeId == 1)
            {
                return RedirectToAction("Confirmation", createdVote);
            }
            else
            {
                createdVote = _createdVoteRepository.GetById(createdVote.Id);
                return View("MultipleChoice", createdVote);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
