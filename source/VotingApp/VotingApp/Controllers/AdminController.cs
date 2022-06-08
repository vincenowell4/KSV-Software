using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VotingApp.DAL.Abstract;
using VotingApp.ViewModel;

namespace VotingApp.Controllers
{
    
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        // GET: AdminController
        private readonly ICreatedVoteRepository _createdVoteRepository;
        private readonly IAppLogRepository _appLogRepository;
        public AdminController(ICreatedVoteRepository createdVoteRepo, IAppLogRepository appLogRepository)
        {
            _createdVoteRepository = createdVoteRepo;
            _appLogRepository = appLogRepository;

        }
        public ActionResult Index()
        {
            var vm = new AdminVM();
            vm.logs = _appLogRepository.GetAllAppLogs();
            return View(vm);
        }
        public ActionResult ViewAll()
        {
            
            return View(_createdVoteRepository.GetAll());
        }
        public ActionResult Delete(int id)
        {
            //does not actually doing anything at the moment
            return RedirectToAction("ViewAll");
        }
        public ActionResult Edit(int id)
        {
            //does not actually doing anything at the moment
            var vote = _createdVoteRepository.GetById(id);
            return View("ViewAll");
            //return RedirectToAction("Edit", vote);
        }
    }
}
