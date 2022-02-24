using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VotingApp.DAL.Abstract;

namespace VotingApp.Controllers
{
    
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        // GET: AdminController
        private readonly ICreatedVoteRepository _createdVoteRepository;
        public AdminController(ICreatedVoteRepository createdVoteRepo)
        {
            _createdVoteRepository = createdVoteRepo;
            
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ViewAll()
        {
            
            return View(_createdVoteRepository.GetAll());
        }
        public ActionResult Delete(int id)
        {
            return RedirectToAction("ViewAll");
        }
        public ActionResult Edit(int id)
        {
            var vote = _createdVoteRepository.GetById(id);
            return View("ViewAll");
            //return RedirectToAction("Edit", vote);
        }
    }
}
