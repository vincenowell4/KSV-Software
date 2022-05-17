using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VotingApp.Models;

namespace VotingApp.Controllers
{
    public class HelpController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HelpController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
