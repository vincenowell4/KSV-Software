using System;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using EmailService;
using VotingApp.DAL.Abstract;
using VotingApp.DAL.Concrete;
using VotingApp.Models;
using System.Threading;

namespace Tests_NUnit_Voting_App
{
    public class Tests_Vince
    {
        private Mock<VotingAppDbContext> _mockContext;
        private Mock<DbSet<CreatedVote>> _createdVoteSet;
        private Mock<DbSet<VoteType>> _voteTypesSet;
        private Mock<DbSet<VoteOption>> _voteOptionSet;
        private Mock<DbSet<SubmittedVote>> _submittedVoteSet;
        private Mock<DbSet<VotingUser>> _votingUsersSet;
        private Mock<DbSet<AppLog>> _appLogSet;
        private List<CreatedVote> _createdVotes;
        private List<VoteType> _voteTypes;
        private List<VoteOption> _voteOption;
        private List<SubmittedVote> _submittedVotes;
        private List<VotingUser> _votingUsers;
        private List<AppLog> _appLogs;


        private Mock<DbSet<T>> GetMockDbSet<T>(IQueryable<T> entities) where T : class
        {
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(entities.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(entities.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(entities.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(entities.GetEnumerator());
            return mockSet;
        }
        [SetUp]
        public void Setup()
        {
            _voteTypes = new List<VoteType>()
            {
                new VoteType { Id = 1,VotingType ="Yes/No Vote" ,VoteTypeDescription = "yes/no discription" },
                new VoteType { Id = 2,VotingType =null ,VoteTypeDescription = "null discription" },
                new VoteType { Id = 3,VotingType ="Multiple Choice Vote" ,VoteTypeDescription = "multiple choice description"}
            };

            _createdVotes = new List<CreatedVote>()
            {
                new CreatedVote { Id = 1, VoteType = _voteTypes[0], AnonymousVote = false, UserId = 1, VoteTitle = "Title", VoteDiscription="This is the description", VoteAccessCode = "abc123"},
                new CreatedVote { Id = 2, VoteType = _voteTypes[0], AnonymousVote = true, UserId = 1, VoteTitle = null, VoteDiscription=null, VoteAccessCode = "xyz789"},
                new CreatedVote { Id = 3, VoteType = _voteTypes[2], AnonymousVote = false, UserId = 1, VoteTitle = "Mult Choice Vote", VoteDiscription="Mult choice description", VoteOptions = _voteOption, VoteAccessCode = "l2m4n7"}
            };

            _voteOption = new List<VoteOption>()
            {
                new VoteOption {CreatedVote = _createdVotes[2], CreatedVoteId = 3, Id = 1, VoteOptionString = "option 1"},
                new VoteOption {CreatedVote = _createdVotes[2], CreatedVoteId = 3, Id = 2, VoteOptionString = "option 2"},
                new VoteOption {CreatedVote = _createdVotes[2], CreatedVoteId = 3, Id = 3, VoteOptionString = "option 3"}
            };

            _votingUsers = new List<VotingUser>()
            {
                new VotingUser { Id = 1, NetUserId = "ABC123CBA31", UserName = "name@mail.com" },
                new VotingUser { Id = 2, NetUserId = "123456", UserName = "name2@mail.com" }
            };

            _submittedVotes = new List<SubmittedVote>()
            {
                new SubmittedVote { CreatedVote = _createdVotes[2], CreatedVoteId = 3, Id = 1, VoteChoice = 1, UserId = 1},
            };
            _appLogs = new List<AppLog>()
            {
                new AppLog { Id = 1, Date = DateTime.Today, LogLevel = "Error", LogMessage = "There was an error creating this page"},
                new AppLog { Id = 2, Date = DateTime.Today, LogLevel = "Info", LogMessage = "Successfully created a vote"}
            };

            _voteTypesSet = GetMockDbSet(_voteTypes.AsQueryable());
            _createdVoteSet = GetMockDbSet(_createdVotes.AsQueryable());
            _voteOptionSet = GetMockDbSet(_voteOption.AsQueryable());
            _votingUsersSet = GetMockDbSet(_votingUsers.AsQueryable());
            _submittedVoteSet = GetMockDbSet(_submittedVotes.AsQueryable());
            _appLogSet = GetMockDbSet(_appLogs.AsQueryable());

            _mockContext = new Mock<VotingAppDbContext>();
            _mockContext.Setup(ctx => ctx.VoteTypes).Returns(_voteTypesSet.Object);
            _mockContext.Setup(ctx => ctx.Set<VoteType>()).Returns(_voteTypesSet.Object);
            _mockContext.Setup(ctx => ctx.CreatedVotes).Returns(_createdVoteSet.Object);
            _mockContext.Setup(ctx => ctx.Set<CreatedVote>()).Returns(_createdVoteSet.Object);
            _mockContext.Setup(ctx => ctx.VoteOptions).Returns(_voteOptionSet.Object);
            _mockContext.Setup(ctx => ctx.Set<VoteOption>()).Returns(_voteOptionSet.Object);
            _mockContext.Setup(ctx => ctx.VotingUsers).Returns(_votingUsersSet.Object);
            _mockContext.Setup(ctx => ctx.Set<VotingUser>()).Returns(_votingUsersSet.Object);
            _mockContext.Setup(ctx => ctx.SubmittedVotes).Returns(_submittedVoteSet.Object);
            _mockContext.Setup(ctx => ctx.Set<SubmittedVote>()).Returns(_submittedVoteSet.Object);
            _mockContext.Setup(ctx => ctx.AppLogs).Returns(_appLogSet.Object);
            _mockContext.Setup(ctx => ctx.Set<AppLog>()).Returns(_appLogSet.Object);
            _mockContext.Setup(x => x.Add(It.IsAny<CreatedVote>())).Callback<CreatedVote>((s) => _createdVotes.Add(s));
            _mockContext.Setup(x => x.Update(It.IsAny<CreatedVote>())).Callback<CreatedVote>((s) =>
            {
                var found = _createdVotes.FirstOrDefault(a => a.Id == s.Id);
                if (found == null)
                {
                    _createdVotes.Add(s);
                }
                else
                {
                    var item = _createdVotes.Where(a => a.Id == s.Id).First();
                    var index = _createdVotes.IndexOf(item);
                    _createdVotes[index] = s;
                }
            });

        }

        [Test]
        //VA-59 As a user I want to be able to create multi-round votes, so voters can narrow down options
        public void VA59_Test_CreatedVoteRepo_GetAllClosedMultiRoundVotes_ShouldReturnOneRowsWhenThereIsAMultiRoundVoteThatIsClosed()
        {
            //arrange
            DateTime pastDate = DateTime.UtcNow.AddDays(-1);
            EmailConfiguration emailConfig = new EmailConfiguration();
            IEmailSender emailSender = new EmailSender(emailConfig);
            ICreatedVoteRepository cvRepo = new CreatedVoteRepository(_mockContext.Object, emailSender);
            CreatedVote testVote = cvRepo.AddOrUpdate(new CreatedVote
            {
                UserId = 1,
                VoteTitle = "Closed Vote Test",
                VoteDiscription = "Closed Vote Description",
                VoteCloseDateTime = pastDate,
                VoteTypeId = 3,
                AnonymousVote = false,
                NextRoundId = 0
            }); ;

            //act
            IList<CreatedVote> createdVotes = cvRepo.GetAllClosedMultiRoundVotes();

            // assert that a one-day delayed vote created in the test database should not yet have a VoteAccessCode, since it is due to start in one day from now
            Assert.IsTrue(createdVotes.Count == 1);
        }

        [Test]
        //VA-59 As a user I want to be able to create multi-round votes, so voters can narrow down options
        public void VA59_Test_CreatedVoteRepo_GetAllClosedMultiRoundVotes_ShouldReturnNoRowsWhenThereIsOnlyAMultiRoundVoteThatIsStillOpen()
        {
            //arrange
            DateTime futureDate = DateTime.UtcNow.AddDays(+1);
            EmailConfiguration emailConfig = new EmailConfiguration();
            IEmailSender emailSender = new EmailSender(emailConfig);
            ICreatedVoteRepository cvRepo = new CreatedVoteRepository(_mockContext.Object, emailSender);
            CreatedVote testVote = cvRepo.AddOrUpdate(new CreatedVote
            {
                UserId = 1,
                VoteTitle = "Closed Vote Test",
                VoteDiscription = "Closed Vote Description",
                VoteCloseDateTime = futureDate,
                VoteTypeId = 3,
                AnonymousVote = false,
                NextRoundId = 0
            }); ;

            //act
            IList<CreatedVote> createdVotes = cvRepo.GetAllClosedMultiRoundVotes();

            // assert that a one-day delayed vote created in the test database should not yet have a VoteAccessCode, since it is due to start in one day from now
            Assert.IsTrue(createdVotes.Count == 0);
        }

        [Test]
        //VA-86 As a user, I want make a vote delayed, so that I can make it now and have it active for voting later
        public void VA86_Test_Create_Vote_For_Delayed_Vote_GetVoteById_Should_Return_Vote_With_Null_VoteAccessCode()
        {
            // arrange
            DateTime futureDate = DateTime.UtcNow.AddDays(1);
            EmailConfiguration emailConfig = new EmailConfiguration();
            IEmailSender emailSender = new EmailSender(emailConfig);
            ICreatedVoteRepository cvRepo = new CreatedVoteRepository(_mockContext.Object, emailSender);
            CreatedVote testVote = cvRepo.AddOrUpdate(new CreatedVote
            {
                UserId = 1,
                VoteTitle = "Delayed Vote Test",
                VoteDiscription = "Delayed Vote Description",
                //DelayedVote = true,
                //VoteStartDateTime = futureDate,
                VoteCloseDateTime = futureDate.AddHours(1),
                VoteTypeId = 1,
                AnonymousVote = false
            });

            // act
            string voteAccessCode = cvRepo.GetById(testVote.Id).VoteAccessCode; //test vote should not have a VoteAccessCode

            // assert that a one-day delayed vote created in the test database should not yet have a VoteAccessCode, since it is due to start in one day from now
            Assert.IsNull(voteAccessCode);
        }

        [Test]
        //VA-86 As a user, I want make a vote delayed, so that I can make it now and have it active for voting later
        public void VA86_VotingUserRepo_GetUserById_ShouldReturnAVotingUserWhenTheIdIsValid()
        {
            // arrange
            IVotingUserRepositiory repo = new VotingUserRepository(_mockContext.Object);

            // act
            VotingUser votingUser = repo.GetUserById(2);

            // assert
            Assert.IsTrue(votingUser.UserName == "name2@mail.com" && votingUser.NetUserId == "123456");
        }

        [Test]
        //VA-86 As a user, I want make a vote delayed, so that I can make it now and have it active for voting later
        public void VA86_VotingUserRepo_GetUserById_ShouldReturnNullWhenTheIdIsNotValid()
        {
            // arrange
            IVotingUserRepositiory repo = new VotingUserRepository(_mockContext.Object);

            // act
            VotingUser votingUser = repo.GetUserById(3);

            // assert
            Assert.IsTrue(votingUser == null);
        }

        [Test]
        //VA-86 As a user, I want make a vote delayed, so that I can make it now and have it active for voting later
        public void VA86_CreatedVoteRepo_GetAllVotesWithNoAccessCode_ShouldReturnTwoRowsWhenThereAreTwoDelayedVotes()
        {
            // arrange
            EmailConfiguration emailConfig = new EmailConfiguration();
            IEmailSender emailSender = new EmailSender(emailConfig);
            ICreatedVoteRepository repo = new CreatedVoteRepository(_mockContext.Object, emailSender);

            //add two more votes that don't have a Vote Access Code
            repo.AddOrUpdate(new CreatedVote { Id = 4, VoteType = _voteTypes[0], AnonymousVote = true, UserId = 1, VoteTitle = "Yes No Vote", VoteDiscription = "Yes No description" });
            repo.AddOrUpdate(new CreatedVote { Id = 5, VoteType = _voteTypes[2], AnonymousVote = false, UserId = 1, VoteTitle = "Mult Choice Vote", VoteDiscription = "Mult choice description", VoteOptions = _voteOption });


            // act
            IList<CreatedVote> createdVote = repo.GetAllVotesWithNoAccessCode();


            // assert
            Assert.IsTrue(createdVote.Count == 2);
        }


        [Test]
        //VA-86 As a user, I want make a vote delayed, so that I can make it now and have it active for voting later
        public void VA86_CreatedVoteRepo_GetAllVotesWithNoAccessCode_ShouldReturnZeroRowsWhenThereAreNoDelayedVotes()
        {
            // arrange
            EmailConfiguration emailConfig = new EmailConfiguration();
            IEmailSender emailSender = new EmailSender(emailConfig);
            ICreatedVoteRepository repo = new CreatedVoteRepository(_mockContext.Object, emailSender);


            // act
            IList<CreatedVote> createdVote = repo.GetAllVotesWithNoAccessCode(); 


            // assert
            Assert.IsTrue(createdVote.Count == 0);
        }

        [Test]
        //VA-86 As a user, I want make a vote delayed, so that I can make it now and have it active for voting later
        public void VA86_CreationService_AddVoteAccessCode_ShouldReturnCreatedVoteWithVotingAccessCode()
        {
            // arrange
            IVoteOptionRepository voRepo = new VoteOptionRepository(_mockContext.Object);
            EmailConfiguration emailConfig = new EmailConfiguration();
            IEmailSender emailSender = new EmailSender(emailConfig);
            ICreatedVoteRepository createRepo = new CreatedVoteRepository(_mockContext.Object, emailSender);
            IVoteTypeRepository typeRepo = new VoteTypeRepository(_mockContext.Object);
            VoteCreationService voteServ = new VoteCreationService(_mockContext.Object);
            IAppLogRepository appLogRepo = new AppLogRepository(_mockContext.Object);
            CreationService createService = new CreationService(createRepo, typeRepo, voteServ, voRepo, appLogRepo);

            //create a "Delayed Vote" - one that has a Vote Open date, but no Vote Access Code
            var newVote = new CreatedVote
            { Id = 4, VoteTypeId = 1, AnonymousVote = false, UserId = 1, VoteTitle = "Title", VoteDiscription = "Test", VoteOpenDateTime = DateTime.Now };
            createService.Create(ref newVote);

            CreatedVote testVote = createRepo.GetById(4);

            // act
            string testVAC = createService.AddVoteAccessCode(ref testVote);

            // assert
            Assert.True(testVAC.Length > 0);
        }


        [Test]
        //VA-79 As a vote creator, I want others to only be able to cast one vote per account, so that we get accurate vote results
        public void VA79_SubmittedVoteRepo_VoteForRegisteredUserWhoHasntCastVote_ShouldReturnNull()
        {
            // arrange
            ISubmittedVoteRepository repo = new SubmittedVoteRepository(_mockContext.Object);


            // act
            SubmittedVote submittedVote = repo.GetByUserIdAndVoteId(2, 3); //user ID 2 has not voted on vote ID 3; this call should return null


            // assert
            Assert.IsNull(submittedVote);
        }

        [Test]
        //VA-79 As a vote creator, I want others to only be able to cast one vote per account, so that we get accurate vote results
        public void VA79_SubmittedVoteRepo_VoteForRegisteredUserWhoHasCastVote_ShouldReturnSubmittedVoteObject()
        {
            // arrange
            ISubmittedVoteRepository repo = new SubmittedVoteRepository(_mockContext.Object);


            // act
            SubmittedVote submittedVote = repo.GetByUserIdAndVoteId(1, 3); //user ID 1 has voted on vote ID 3; this call should return an object


            // assert
            Assert.IsNotNull(submittedVote);
        }
    }
}
