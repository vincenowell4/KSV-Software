using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VotingApp.DAL.Abstract;
using Microsoft.AspNetCore.Identity;
using VotingApp.ViewModel;

namespace VotingApp.Controllers
{
    public class AccessController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICreatedVoteRepository _createdVoteRepository;
        private readonly IVoteTypeRepository _voteTypeRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public AccessController(ILogger<HomeController> logger, ICreatedVoteRepository createdVoteRepo, IVoteTypeRepository voteTypeRepository, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _createdVoteRepository = createdVoteRepo;
            _voteTypeRepository = voteTypeRepository;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            
            return View();
        }

        [HttpGet]
        public IActionResult Access(string code)
        {
            SubmitVoteVM model = new SubmitVoteVM();
            model.vote = _createdVoteRepository.GetVoteByAccessCode(code);
            if (model.vote != null)
            {
                return View("SubmitVote", model);
            }
            else
            {
                return View("Index");
            }
            
        }

        [HttpGet]
        public IActionResult CastVote(string choice)
        {
            var userId = _userManager.GetUserId(User);
            return View("SubmitVote", choice);
        }

    }
}
