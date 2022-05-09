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
        private readonly IAppLogRepository _appLogRepository;

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
            EmailService.IEmailSender emailSender,
            IAppLogRepository appLogRepository)
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
            _appLogRepository = appLogRepository;
        }

        [HttpGet("novac/all")]
        public ActionResult GetAllNoVAC()
        {
            IList<CreatedVote> noVACvotes = new List<CreatedVote>();
            noVACvotes = _createdVoteRepository.GetAllVotesWithNoAccessCode();

            IList<CreatedVoteModel> vmNoVACvotes = new List<CreatedVoteModel>();

            for (int i = 0; i < noVACvotes.Count; i++)
            {
                CreatedVoteModel cvVM = new CreatedVoteModel();
                cvVM.Id = noVACvotes[i].Id;
                cvVM.VoteOpenDateTime = noVACvotes[i].VoteOpenDateTime;
                cvVM.TimeZone = noVACvotes[i].TimeZone.TimeName;
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

        [HttpGet("cmv/all")]
        public ActionResult GetAllClosedMultiVotes()
        {
            IList<CreatedVote> cmVotes = new List<CreatedVote>();
            cmVotes = _createdVoteRepository.GetAllClosedMultiRoundVotes();

            IList<MultiRoundVoteModel> vmCMVotes = new List<MultiRoundVoteModel>();

            for (int i = 0; i < cmVotes.Count; i++)
            {
                MultiRoundVoteModel vmCMV = new MultiRoundVoteModel();
                vmCMV.Id = cmVotes[i].Id;
                vmCMVotes.Add(vmCMV);
            }

            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<Program>().Build();

            string apiKey = config.GetSection("VotingAppApiKey").Value;

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(vmCMVotes);
            return Content(json);
        }

        [HttpGet("vr/{id}")]
        public ActionResult GetMultiRoundVoteResults(int id)
        {
            IList<VoteOption> vOpts = _voteOptionRepository.GetAllByVoteID(id);

            Dictionary<VoteOption, int> totVotesForOptions = _submittedVoteRepository.TotalVotesForEachOption(id, vOpts);

            List<MultiRoundVoteResultsModel> mrVoteResults = new List<MultiRoundVoteResultsModel>();
            List<MultiRoundVoteResultsModel> mrVoteResultsSorted = new List<MultiRoundVoteResultsModel>();
            int voteCount = 0;

            for (int i = 0; i < vOpts.Count; i++)
            {
                MultiRoundVoteResultsModel mrVoteResult = new MultiRoundVoteResultsModel();
                mrVoteResult.VoteOpt = vOpts[i].VoteOptionString;
                bool hasCount = totVotesForOptions.TryGetValue(vOpts[i], out voteCount);
                mrVoteResult.VoteOptCount = voteCount;
                voteCount = 0;
                mrVoteResults.Add(mrVoteResult);
            }

            mrVoteResultsSorted = mrVoteResults.OrderByDescending(p => p.VoteOptCount).ToList();

            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<Program>().Build();

            string apiKey = config.GetSection("VotingAppApiKey").Value;

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(mrVoteResultsSorted);
            return Content(json);
        }

        [HttpGet("nextround/{id}")]
        public ActionResult CreateNextRoundVoteForId(int id)
        {
            IList<VoteOption> vOpts = _voteOptionRepository.GetAllByVoteID(id);

            Dictionary<VoteOption, int> totVotesForOptions = _submittedVoteRepository.TotalVotesForEachOption(id, vOpts);

            List<MultiRoundVoteResultsModel> mrVoteResults = new List<MultiRoundVoteResultsModel>();
            List<MultiRoundVoteResultsModel> mrVoteResultsSorted = new List<MultiRoundVoteResultsModel>();
            int voteCount = 0;

            for (int i = 0; i < vOpts.Count; i++)
            {
                MultiRoundVoteResultsModel mrVoteResult = new MultiRoundVoteResultsModel();
                mrVoteResult.VoteOpt = vOpts[i].VoteOptionString;
                bool hasCount = totVotesForOptions.TryGetValue(vOpts[i], out voteCount);
                mrVoteResult.VoteOptCount = voteCount;
                voteCount = 0;
                mrVoteResults.Add(mrVoteResult);
            }

            mrVoteResultsSorted = mrVoteResults.OrderByDescending(p => p.VoteOptCount).ToList();

            CreatedVote currRound = _createdVoteRepository.GetById(id);
            string roundDuration = _createdVoteRepository.GetMultiRoundVoteDuration(id);
            int roundDays = 0;
            int roundHours = 0;
            int roundMinutes = 0;
            if(roundDuration.Length > 0)
            {
                string[] duration = roundDuration.Split(',');
                if (duration[0].Length > 0)
                    roundDays = int.Parse(duration[0]);
                if (duration[1].Length > 0)
                    roundHours = int.Parse(duration[1]);
                if (duration[2].Length > 0)
                    roundMinutes = int.Parse(duration[2]);
            }
            CreatedVote nextRound = new CreatedVote();

            nextRound.UserId = currRound.UserId;
            nextRound.RoundNumber = currRound.RoundNumber + 1;
            nextRound.RoundDays = roundDays;
            nextRound.RoundHours = roundHours;
            nextRound.RoundMinutes = roundMinutes;
            if (currRound.VoteTitle.Contains("- round") )
            {
                nextRound.VoteTitle = currRound.VoteTitle.Replace(("- round " + ((int)currRound.RoundNumber).ToString()), ("- round " + ((int)currRound.RoundNumber + 1).ToString()));
            } else
            {
                nextRound.VoteTitle = currRound.VoteTitle + " - round " + ((int)currRound.RoundNumber + 1).ToString();
            }
            nextRound.VoteDiscription = currRound.VoteDiscription;
            nextRound.AnonymousVote = currRound.AnonymousVote;
            nextRound.VoteTypeId = currRound.VoteTypeId;
            nextRound.TimeZone = currRound.TimeZone;
            nextRound.VoteOpenDateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, currRound.TimeZone.TimeName);
            nextRound.VoteCloseDateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, currRound.TimeZone.TimeName).AddDays(roundDays).AddHours(roundHours).AddMinutes(roundMinutes);
            nextRound.PrivateVote = currRound.PrivateVote;

            nextRound = _createdVoteRepository.AddOrUpdate(nextRound);
            nextRound.NextRoundId = 0;

            string accCode = "";

            if (nextRound != null)
            {
                currRound.NextRoundId = nextRound.Id;
                _createdVoteRepository.AddOrUpdate(currRound);

                if (mrVoteResultsSorted.Count > 0)
                {
                    for (int i = 0; i < mrVoteResultsSorted.Count - 1; i++)
                    {
                        VoteOption voteOption = new VoteOption();
                        voteOption.CreatedVoteId = nextRound.Id;
                        voteOption.VoteOptionString = mrVoteResultsSorted[i].VoteOpt.ToString();
                        nextRound.VoteOptions.Add(voteOption);
                    }
                }
                nextRound = _createdVoteRepository.AddOrUpdate(nextRound);
                accCode = _creationService.AddVoteAccessCode(ref nextRound);
            }

            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<Program>().Build();

            string apiKey = config.GetSection("VotingAppApiKey").Value;

            string json = "[]";
            if (accCode.Length > 0)
                json = Newtonsoft.Json.JsonConvert.SerializeObject(nextRound.Id);
            return Content(json);
        }

        [HttpGet("setvotedone/{id}")]
        public ActionResult SetVoteDoneForId(int id)
        {
            string voteStatus = "";
            if (id > 0)
            {
                CreatedVote createdVote = _createdVoteRepository.GetById(id);
                if (createdVote != null)
                {
                    createdVote.NextRoundId = -1;
                    _createdVoteRepository.AddOrUpdate(createdVote);
                    voteStatus = (createdVote.NextRoundId == -1 ? "Done" : "");
                }
            }
            return Content(voteStatus);
        }

        [Route("/votewonemail/{id}")]
        public IActionResult SendVoteWonEmailForId(int id)
        {
            string userEmail = "";
            if (id > 0)
            {
                CreatedVote createdVote = _createdVoteRepository.GetById(id);
                if (createdVote != null && createdVote.UserId != null)
                {
                    userEmail = (_votingUserRepository.GetUserById(createdVote.UserId.Value)).UserName;

                    var submitVoteUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/Access/Results?code={createdVote.VoteAccessCode}";

                    var message = new Message(new string[] { userEmail }, AppStrings.EmailSubjectMultiRoundVoteResults,
                        AppStrings.EmailMessageWinningVote + $"<br/><br/>Title: '{createdVote.VoteTitle}'<br/>Description: '{createdVote.VoteDiscription}'<br/>Access Code: {createdVote.VoteAccessCode}<br/><br/>Click <a href='{HtmlEncoder.Default.Encode(submitVoteUrl)}'>here</a> to go to the Vote Results page for this access code");

                    _emailSender.SendEmail(message);

                    Console.WriteLine(userEmail);
                }
            }
            return Content(userEmail);
        }

        [Route("/votetieemail/{id}")]
        public IActionResult SendVoteTieEmailForId(int id)
        {
            string userEmail = "";
            if (id > 0)
            {
                CreatedVote createdVote = _createdVoteRepository.GetById(id);
                if (createdVote != null && createdVote.UserId != null)
                {
                    userEmail = (_votingUserRepository.GetUserById(createdVote.UserId.Value)).UserName;

                    var submitVoteUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/Access/Results?code={createdVote.VoteAccessCode}";

                    var message = new Message(new string[] { userEmail }, AppStrings.EmailSubjectMultiRoundVoteResults,
                        AppStrings.EmailMessageTieVote + $"<br/><br/>Title: '{createdVote.VoteTitle}'<br/>Description: '{createdVote.VoteDiscription}'<br/>Access Code: {createdVote.VoteAccessCode}<br/><br/>Click <a href='{HtmlEncoder.Default.Encode(submitVoteUrl)}'>here</a> to go to the Vote Results page for this access code");

                    _emailSender.SendEmail(message);

                    Console.WriteLine(userEmail);
                }
            }
            return Content(userEmail);
        }

        [Route("/zerovotesemail/{id}")]
        public IActionResult SendZeroVotesEmailForId(int id)
        {
            string userEmail = "";
            if (id > 0)
            {
                CreatedVote createdVote = _createdVoteRepository.GetById(id);
                if (createdVote != null && createdVote.UserId != null)
                {
                    userEmail = (_votingUserRepository.GetUserById(createdVote.UserId.Value)).UserName;

                    var submitVoteUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/Access/Results?code={createdVote.VoteAccessCode}";

                    var message = new Message(new string[] { userEmail }, AppStrings.EmailSubjectMultiRoundVoteResults,
                        AppStrings.EmailMessageZeroVotes + $"<br/><br/>Title: '{createdVote.VoteTitle}'<br/>Description: '{createdVote.VoteDiscription}'<br/>Access Code: {createdVote.VoteAccessCode}<br/><br/>Click <a href='{HtmlEncoder.Default.Encode(submitVoteUrl)}'>here</a> to go to the Vote Results page for this access code");

                    _emailSender.SendEmail(message);

                    Console.WriteLine(userEmail);
                }
            }
            return Content(userEmail);
        }

        [Route("/nextroundemail/{id}")]
        public IActionResult SendNextRoundEmailForId(int id)
        {
            string userEmail = "";
            if (id > 0)
            {
                CreatedVote createdVote = _createdVoteRepository.GetById(id);
                if (createdVote != null && createdVote.UserId != null)
                {
                    userEmail = (_votingUserRepository.GetUserById(createdVote.UserId.Value)).UserName;

                    var submitVoteUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/Access/Access?code={createdVote.VoteAccessCode}";

                    var message = new Message(new string[] { userEmail }, AppStrings.EmailSubjectMultiRoundNewVoteOpen,
                        AppStrings.EmailMessageNextRoundVoteOpen + $"<br/><br/>Title: '{createdVote.VoteTitle}'<br/>Description: '{createdVote.VoteDiscription}'<br/>Access Code: {createdVote.VoteAccessCode}<br/><br/>Click <a href='{HtmlEncoder.Default.Encode(submitVoteUrl)}'>here</a> to go to the Cast Vote page for this access code");

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

    public class CreatedVoteModel
    {
        public int Id { get; set; }
        public DateTime? VoteOpenDateTime { get; set; }

        public string TimeZone { get; set; }
    }

    public class MultiRoundVoteModel
    {
        public int Id { get; set; }
    }

    public class MultiRoundVoteResultsModel
    {
        public string VoteOpt {  get; set; }
        public int VoteOptCount { get; set; }
    }
    public class MultiRoundVoteClosedModel
    {
        public int Id { get; set; }
        public int NextRoundId { get; set; }
    }
}

