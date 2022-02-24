using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using VotingApp.DAL.Abstract;
using VotingApp.Models;
using VotingApp.ViewModel;

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

        public CreateController(ILogger<HomeController> logger, 
            ICreatedVoteRepository createdVoteRepo, 
            IVoteTypeRepository voteTypeRepository, 
            VoteCreationService voteCreationService, 
            UserManager<IdentityUser> userManager,
            IVotingUserRepositiory votingUserRepositiory)
        {
            _logger = logger;
            _createdVoteRepository = createdVoteRepo;
           _voteTypeRepository = voteTypeRepository;
            _voteCreationService = voteCreationService;
            _userManager = userManager;
            _votingUserRepository = votingUserRepositiory;
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
        public IActionResult Index([Bind("VoteTypeId,VoteTitle,VoteDiscription,Anonymous")]CreatedVote createdVote)
        {
            ModelState.Remove("VoteType");
            ModelState.Remove("VoteAccessCode");
            
            if (User.Identity.IsAuthenticated != false)
            {
                createdVote.User = _votingUserRepository.GetUserByAspId(_userManager.GetUserId(User));
            }
            if (ModelState.IsValid)
            {
                if (createdVote.VoteTypeId == 1)
                {   
                    createdVote.VoteOptions = _voteTypeRepository.CreateVoteOptions();
                }
                try
                {

                    createdVote.VoteAccessCode = _voteCreationService.generateCode();
                    _createdVoteRepository.AddOrUpdate(createdVote);
                }
                catch (DbUpdateConcurrencyException e)
                {
                    ViewBag.Message = "A concurrency error occurred while trying to create the expedition. Please try again";
                    return View(createdVote);
                }
                catch (DbUpdateException e)
                {
                    ViewBag.Message = "An unknown database error occurred while trying to create the item. Please try again";
                    return View(createdVote);
                }

                
                    
                return RedirectToAction("Confirmation", createdVote);

            }
            else
            {
                ViewBag.Message = "An unknown database error occurred while trying to create the item. Please try again.";
                return View(createdVote);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult edit([Bind("Id,VoteTypeId,VoteTitle,VoteDiscription,Anonymous")] CreatedVote createdVote)
        {
            ModelState.Remove("VoteType");

            if (ModelState.IsValid)
            {
                try
                {
                    _createdVoteRepository.AddOrUpdate(createdVote);
                    _createdVoteRepository.AddOrUpdate(createdVote);
                }
                catch (DbUpdateConcurrencyException e)
                {
                    ViewBag.Message = "A concurrency error occurred while trying to create the expedition. Please try again";
                    return View(createdVote);
                }
                catch (DbUpdateException e)
                {
                    ViewBag.Message = "An unknown database error occurred while trying to create the item. Please try again";
                    return View(createdVote);
                }
                return RedirectToAction("Confirmation", createdVote);
            }
            else
            {
                ViewBag.Message = "An unknown database error occurred while trying to create the item. Please try again.";
                return View(createdVote);
            }
        }

        [HttpGet]
        public IActionResult Confirmation(CreatedVote createdVote)
        {
            var vm = new ConfirmationVM();
            vm.VoteTitle = _createdVoteRepository.GetVoteTitle(createdVote.Id);
            vm.VoteDescription = _createdVoteRepository.GetVoteDescription(createdVote.Id);
            vm.VoteType = _voteTypeRepository.GetVoteType(createdVote.VoteTypeId);
            vm.ChosenVoteDescriptionHeader = _voteTypeRepository.GetChosenVoteHeader(vm.VoteType);
            vm.VotingOptions = _voteTypeRepository.GetVoteOptions(vm.VoteType);
            vm.ID = createdVote.Id;
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
