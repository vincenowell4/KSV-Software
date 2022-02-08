using Microsoft.AspNetCore.Mvc;
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

        public CreateController(ILogger<HomeController> logger, ICreatedVoteRepository createdVoteRepo)
        {
            _logger = logger;
            _createdVoteRepository = createdVoteRepo;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(CreatedVote createdVote)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _createdVoteRepository.AddOrUpdate(createdVote);
                    createdVote.Anonymous = _createdVoteRepository.SetAnonymous(createdVote.Id);
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
            var vm = new CreateVM(); 
            vm.VoteDescription = _createdVoteRepository.GetVoteDescription(createdVote.Id);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Confirmation()
        {
            return RedirectToAction("Index", "Home");
        }

        //[HttpGet]
        public IActionResult BackToIndexPage()
        {
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
