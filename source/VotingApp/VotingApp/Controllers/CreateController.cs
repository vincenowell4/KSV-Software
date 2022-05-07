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
using System.Linq;

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
        private readonly GoogleTtsService _googleTtsService;
        private readonly IAppLogRepository _appLogRepository;
        private readonly ITimeZoneRepo _timeZoneRepo;

        public CreateController(ILogger<HomeController> logger, 
            ICreatedVoteRepository createdVoteRepo, 
            IVoteTypeRepository voteTypeRepository, 
            VoteCreationService voteCreationService, 
            UserManager<IdentityUser> userManager,
            IVotingUserRepositiory votingUserRepositiory,
            IVoteOptionRepository voteOptionRepository,
            CreationService creationService,
            ISubmittedVoteRepository submittedVoteRepository,
            IVoteAuthorizedUsersRepo voteAuthorizedUsersRepo, 
            GoogleTtsService googleTtsService,
            IAppLogRepository appLogRepository,
            ITimeZoneRepo timeZoneRepo)

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
            _googleTtsService = googleTtsService;
            _appLogRepository = appLogRepository;
            _timeZoneRepo = timeZoneRepo;
        }

        [HttpGet]
        public IActionResult Index()
        {
            SelectList selectListVoteType = null;
            SelectList timeZoneList = null;
            timeZoneList = new SelectList( _timeZoneRepo.GetAllTimeZones().Select(x => new {Text = $"{x.TimeName}", Value = x.Id}), "Value", "Text");
            if (User.Identity.IsAuthenticated != false)
            {
                selectListVoteType = new SelectList(
                    _voteTypeRepository.VoteTypes().Select(a => new { Text = $"{a.VotingType}", Value = a.Id }),
                    "Value", "Text");
            }
            else
            { //if user is not logged in, then Multi-Round voting is not available
                selectListVoteType = new SelectList(
                    _voteTypeRepository.VoteTypes().Select(a => new { Text = $"{a.VotingType}", Value = a.Id }).Where(o => o.Text != "Multiple Choice Multi-Round Vote"),
                    "Value", "Text");
            }

            ViewData["VoteTypeId"] = selectListVoteType;
            ViewData["TimeZoneId"] = timeZoneList;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index([Bind("VoteTypeId,VoteTitle,VoteDiscription,AnonymousVote,VoteOpenDateTime,VoteCloseDateTime, PrivateVote, TimeZoneId")]CreatedVote createdVote)
        {
            
            ModelState.Remove("VoteType");
            ModelState.Remove("VoteAccessCode");
            ModelState.Remove("VoteAudioBytes");
            ModelState.Remove("TimeZone");
            SelectList selectListVoteType = null;
            SelectList timeZoneList = null;
            timeZoneList = new SelectList(_timeZoneRepo.GetAllTimeZones().Select(x => new { Text = $"{x.TimeName}", Value = x.Id }), "Value", "Text");
            if (User.Identity.IsAuthenticated != false)
            {
                selectListVoteType = new SelectList(
                    _voteTypeRepository.VoteTypes().Select(a => new { Text = $"{a.VotingType}", Value = a.Id }),
                    "Value", "Text");
            }
            else
            { //if user is not logged in, then Multi-Round voting is not available
                selectListVoteType = new SelectList(
                    _voteTypeRepository.VoteTypes().Select(a => new { Text = $"{a.VotingType}", Value = a.Id }).Where(o => o.Text != "Multiple Choice Multi-Round Vote"),
                    "Value", "Text");
            }
            ViewData["VoteTypeId"] = selectListVoteType;
            ViewData["TimeZoneId"] = timeZoneList;
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
                _appLogRepository.LogError("An unknown error occurred while trying to create this vote with id: " + createdVote.Id);
                return View(createdVote);
            }
        }

        public IActionResult AddVoteUsers(AuthorizedUserPageVM vm)
        {
            return View("AddVoteUsers",vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult edit([Bind("Id,VoteTypeId,VoteTitle,VoteDiscription,AnonymousVote,VoteOption,VoteOpenDateTime,VoteCloseDateTime,VoteAccessCode, PrivateVote, TimeZoneId")] CreatedVote createdVote, int oldVoteTypeId)
        {
            ModelState.Remove("VoteType");
            ModelState.Remove("VoteAccessCode");
            ModelState.Remove("VoteAudioBytes");
            ModelState.Remove("TimeZone");
            SelectList selectListVoteType = null;
            SelectList timeZoneList = null;
            timeZoneList = new SelectList(_timeZoneRepo.GetAllTimeZones().Select(x => new { Text = $"{x.TimeName}", Value = x.Id }), "Value", "Text");
            if (User.Identity.IsAuthenticated != false)
            {
                selectListVoteType = new SelectList(
                    _voteTypeRepository.VoteTypes().Select(a => new { Text = $"{a.VotingType}", Value = a.Id }),
                    "Value", "Text");
            }
            else
            { //if user is not logged in, then Multi-Round voting is not available
                selectListVoteType = new SelectList(
                    _voteTypeRepository.VoteTypes().Select(a => new { Text = $"{a.VotingType}", Value = a.Id }).Where(o => o.Text != "Multiple Choice Multi-Round Vote"),
                    "Value", "Text");
            }
            ViewData["VoteTypeId"] = selectListVoteType;
            ViewData["TimeZoneId"] = timeZoneList;
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
                _appLogRepository.LogError("An unknown error occurred while trying to edit created vote id: " + createdVote.Id);
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
            if (option == null)
            {
                _appLogRepository.LogError("Error adding null vote option to created vote id: " + vote.Id);
                return RedirectToAction("MultipleChoice", vote);
            }
            else
            {
                vote.VoteOptions.Add(voteOption);
                _createdVoteRepository.AddOrUpdate(vote);
                return RedirectToAction("MultipleChoice", vote);
            }
        }
        public ActionResult LoadAudio(int id)
        {
            var vote = _createdVoteRepository.GetById(id);
            if (vote.VoteAudioBytes == null)
            {
                vote.VoteAudioBytes = _googleTtsService.CreateVoteAudio(vote);
                _createdVoteRepository.AddOrUpdate(vote);
            }
            var audioBytes = vote.VoteAudioBytes;
            return base.File(audioBytes, "audio/wav");
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

            createdVote = _createdVoteRepository.GetById(createdVote.Id);
            createdVote.VoteAudioBytes = _googleTtsService.CreateVoteAudio(createdVote);
            //_googleTtsService.CreateAudioFiles(createdVote);
            createdVote = _createdVoteRepository.AddOrUpdate(createdVote);
            if (createdVote.PrivateVote)
            {
                var accessCode = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/Access/{createdVote.VoteAccessCode}";
                var listOfEmails = createdVote.VoteAuthorizedUsers.ToList();
                _createdVoteRepository.SendEmails(listOfEmails, createdVote, accessCode);
            }
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
            vm.VoteCloseDateTime = createdVote.VoteCloseDateTime ?? TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, createdVote
                .TimeZone.TimeName);
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

                var votesForUser = _createdVoteRepository.GetAllForUserId(userId);
                createdVotesVM.OpenVotes = _createdVoteRepository.GetOpenCreatedVotes(votesForUser);
                createdVotesVM.ClosedVotes = _createdVoteRepository.GetClosedCreatedVotes(votesForUser);
                //

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
        public IActionResult VoteResultsButton(int id)
        {
            //var createdVote = _createdVoteRepository.GetById(id);
            return RedirectToAction("VoteResults", new {id = id});
        }

        [HttpGet]
        public IActionResult VoteResults(int id)
        {
            var createdVote = _createdVoteRepository.GetById(id);
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
            vm.ChartVoteTotals = _submittedVoteRepository.TotalVotesPerOption(createdVote.Id, vm.VoteOptions);
            vm.ChartVoteOptions = _submittedVoteRepository.MatchingOrderOptionsList(createdVote.Id, vm.VoteOptions);
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
            SelectList selectListVoteType = null;
            SelectList timeZoneList = null;
            timeZoneList = new SelectList(_timeZoneRepo.GetAllTimeZones().Select(x => new { Text = $"{x.TimeName}", Value = x.Id }), "Value", "Text");
            if (User.Identity.IsAuthenticated != false)
            {
                selectListVoteType = new SelectList(
                    _voteTypeRepository.VoteTypes().Select(a => new { Text = $"{a.VotingType}", Value = a.Id }),
                    "Value", "Text");
            }
            else
            { //if user is not logged in, then Multi-Round voting is not available
                selectListVoteType = new SelectList(
                    _voteTypeRepository.VoteTypes().Select(a => new { Text = $"{a.VotingType}", Value = a.Id }).Where(o => o.Text != "Multiple Choice Multi-Round Vote"),
                    "Value", "Text");
            }
            ViewData["VoteTypeId"] = selectListVoteType;
            ViewData["TimeZoneId"] = timeZoneList;
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
            _appLogRepository.LogError("Error - RequestId = " + Activity.Current?.Id);
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
