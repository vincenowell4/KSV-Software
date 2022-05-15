using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace VotingApp.Controllers
{
    [Route("error")]
    public class ErrorController : Controller
    {
        [Route("404")]
        public IActionResult Error404()
        {
            return View();
        }

        [Route("500")]
        public IActionResult Error500()
        {
            return View();
        }
    }
 }
