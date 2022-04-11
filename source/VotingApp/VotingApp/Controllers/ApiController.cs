using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using VotingApp.DAL.Abstract;
using VotingApp.Models;
using EmailService;
using VotingApp.ViewModel;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http;
using System.Text.Encodings.Web;
using VotingApp.Data;
using VotingApp.Attributes;

namespace VotingApp.Controllers
{
    [ApiKey]
    public class ApiController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICreatedVoteRepository _createdVoteRepository;
        private readonly IVoteTypeRepository _voteTypeRepository;
        private readonly VoteCreationService _voteCreationService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IVotingUserRepositiory _votingUserRepository;
        private readonly IVoteOptionRepository _voteOptionRepository;
        private readonly CreationService _creationService;
        private readonly ISubmittedVoteRepository _submittedVoteRepository;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly EmailService.IEmailSender _emailSender;
        private readonly IUserStore<IdentityUser> _userStore;

        public ApiController(ILogger<HomeController> logger,
            ICreatedVoteRepository createdVoteRepo,
            IVoteTypeRepository voteTypeRepository,
            VoteCreationService voteCreationService,
            UserManager<IdentityUser> userManager,
            IVotingUserRepositiory votingUserRepositiory,
            IVoteOptionRepository voteOptionRepository,
            CreationService creationService,
            ISubmittedVoteRepository submittedVoteRepository,
            IUserStore<IdentityUser> userStore,
            EmailService.IEmailSender emailSender)
        {
            _logger = logger;
            _createdVoteRepository = createdVoteRepo;
            _voteTypeRepository = voteTypeRepository;
            _voteCreationService = voteCreationService;
            _userManager = userManager;
            _votingUserRepository = votingUserRepositiory;
            _voteOptionRepository = voteOptionRepository;
            _creationService = creationService;
            _submittedVoteRepository = submittedVoteRepository;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _emailSender = emailSender;
        }

        [HttpGet("novac/all")]
        public ActionResult GetAllNoVAC()
        {
            IList<CreatedVote> noVACvotes = new List<CreatedVote>();
            noVACvotes = _createdVoteRepository.GetAllVotesWithNoAccessCode();

            IList<CreatedVoteViewModel> vmNoVACvotes = new List<CreatedVoteViewModel>();

            for (int i = 0; i < noVACvotes.Count; i++)
            {
                CreatedVoteViewModel cvVM = new CreatedVoteViewModel();
                cvVM.Id = noVACvotes[i].Id;
                cvVM.VoteOpenDateTime = noVACvotes[i].VoteOpenDateTime;
                vmNoVACvotes.Add(cvVM);
            }

            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<Program>().Build();

            string apiKey = config.GetSection("VotingAppApiKey").Value;

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(vmNoVACvotes);
            return Content(json);

        }

        [Route("/genvac/{id}")]
        public IActionResult SetVACForId(int id)
        {
            string accCode = "";
            if (id > 0)
            {
                CreatedVote createdVote = _createdVoteRepository.GetById(id);
                if (createdVote != null)
                {
                    accCode = _creationService.AddVoteAccessCode(ref createdVote);
                }
            }
            return Content(accCode);
        }

        [Route("/genemail/{id}")]
        public IActionResult SendEmailForId(int id)
        {
            string userEmail = "";
            if (id > 0)
            {
                CreatedVote createdVote = _createdVoteRepository.GetById(id);
                if (createdVote != null && createdVote.UserId != null)
                {
                    userEmail = (_votingUserRepository.GetUserById(createdVote.UserId.Value)).UserName;

                    var submitVoteUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/Access/Access?code={createdVote.VoteAccessCode}";

                    var message = new Message(new string[] { userEmail }, AppStrings.EmailSubjectVoteOpen,
                        AppStrings.EmailMessageVoteOpen + $"<br/><br/>Title: '{createdVote.VoteTitle}'<br/>Description: '{createdVote.VoteDiscription}'<br/>Access Code: {createdVote.VoteAccessCode}<br/><br/>Click <a href='{HtmlEncoder.Default.Encode(submitVoteUrl)}'>here</a> to go to the Cast Vote page for this access code");

                    _emailSender.SendEmail(message);

                    Console.WriteLine(userEmail);
                }
            }
            return Content(userEmail);
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }

    public class CreatedVoteViewModel
    {
        public int Id { get; set; }
        public DateTime? VoteOpenDateTime { get; set; }
    }
}

