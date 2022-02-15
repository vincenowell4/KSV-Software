using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
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

        public CreateController(ILogger<HomeController> logger, ICreatedVoteRepository createdVoteRepo, IVoteTypeRepository voteTypeRepository)
        {
            _logger = logger;
            _createdVoteRepository = createdVoteRepo;
           _voteTypeRepository = voteTypeRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var selectListVoteType = new SelectList(
                _voteTypeRepository.VoteTypes().Select(a => new { Text = $"{a.Type}", Value = a.Id }),
                "Value", "Text");
            ViewData["VoteTypeId"] = selectListVoteType;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index([Bind("VoteTypeId,VoteDiscription,Anonymous")]CreatedVote createdVote)
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
                    ViewBag.Message = "A concurrency error occured while trying to create the expedition. Please try again";
                    return View(createdVote);
                }
                catch (DbUpdateException e)
                {
                    ViewBag.Message = "An unknown database error occured while trying to create the item. Please try again";
                    return View(createdVote);
                }
                return RedirectToAction("Confirmation", createdVote);
            }
            else
            {
                ViewBag.Message = "An unknown datbase error occured while trying to create the item. Please try again.";
                return View(createdVote);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult edit([Bind("Id,VoteTypeId,VoteDiscription,Anonymous")] CreatedVote createdVote)
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
                    ViewBag.Message = "A concurrency error occured while trying to create the expedition. Please try again";
                    return View(createdVote);
                }
                catch (DbUpdateException e)
                {
                    ViewBag.Message = "An unknown database error occured while trying to create the item. Please try again";
                    return View(createdVote);
                }
                return RedirectToAction("Confirmation", createdVote);
            }
            else
            {
                ViewBag.Message = "An unknown datbase error occured while trying to create the item. Please try again.";
                return View(createdVote);
            }
        }

        [HttpGet]
        public IActionResult Confirmation(CreatedVote createdVote)
        {
            var vm = new ConfirmationVM(); 
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
                _voteTypeRepository.VoteTypes().Select(a => new { Text = $"{a.Type}", Value = a.Id }),
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
