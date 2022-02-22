using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VotingApp.DAL.Abstract;
using VotingApp.ViewModel;

namespace VotingApp.Controllers
{
    public class AccessController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICreatedVoteRepository _createdVoteRepository;
        private readonly IVoteTypeRepository _voteTypeRepository;

        public AccessController(ILogger<HomeController> logger, ICreatedVoteRepository createdVoteRepo, IVoteTypeRepository voteTypeRepository)
        {
            _logger = logger;
            _createdVoteRepository = createdVoteRepo;
            _voteTypeRepository = voteTypeRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            
            return View();
        }

        [HttpGet]
        public IActionResult Access(int code)
        {
            SubmitVoteVM model = new SubmitVoteVM();
            model.vote = _createdVoteRepository.GetById(code);
            if (model.vote != null)
            {
                string type = _voteTypeRepository.GetVoteType(code);
                model.options = _voteTypeRepository.GetVoteOptions(type);
            }
            else
            {
                return View("Index");
            }
            return View("SubmitVote", model);
        }

        [HttpGet]
        public IActionResult CastVote(string choice)
        {
            
            return View("SubmitVote", choice);
        }

    }
}
