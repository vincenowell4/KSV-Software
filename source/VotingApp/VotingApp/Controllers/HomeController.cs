using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VotingApp.DAL.Abstract;
using VotingApp.Models;
using System.Reflection;

namespace VotingApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAppLogRepository _appLogRepository;

        public HomeController(ILogger<HomeController> logger, IAppLogRepository appLogRepository)
        {
            _logger = logger;
            _appLogRepository = appLogRepository;   
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            MethodBase method = MethodBase.GetCurrentMethod();
            _appLogRepository.LogError(method.ReflectedType.Name, method.Name, "Error - RequestId = " + Activity.Current?.Id ?? HttpContext.TraceIdentifier);

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}