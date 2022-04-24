using System;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using VotingApp.DAL.Abstract;
using VotingApp.DAL.Concrete;
using VotingApp.Models;
using System.Threading;
using EmailService;

namespace Tests_NUnit_Voting_App
{
    public class VA86Tests
    {
        private Mock<VotingAppDbContext> _mockContext;
        private Mock<DbSet<CreatedVote>> _createdVoteSet;
        private Mock<DbSet<VoteType>> _voteTypesSet;
        private Mock<DbSet<VoteOption>> _voteOptionSet;
        private Mock<DbSet<SubmittedVote>> _submittedVoteSet;
        private Mock<DbSet<VotingUser>> _votingUsersSet;
        private List<CreatedVote> _createdVotes;
        private List<VoteType> _voteTypes;
        private List<VoteOption> _voteOption;
        private List<SubmittedVote> _submittedVotes;
        private List<VotingUser> _votingUsers;


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
                new CreatedVote { Id = 2, VoteType = _voteTypes[0], AnonymousVote = true, UserId = 1, VoteTitle = null, VoteDiscription=null},
                new CreatedVote { Id = 3, VoteType = _voteTypes[2], AnonymousVote = false, UserId = 1, VoteTitle = "Mult Choice Vote", VoteDiscription="Mult choice description", VoteOptions = _voteOption}
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

            _voteTypesSet = GetMockDbSet(_voteTypes.AsQueryable());
            _createdVoteSet = GetMockDbSet(_createdVotes.AsQueryable());
            _voteOptionSet = GetMockDbSet(_voteOption.AsQueryable());
            _votingUsersSet = GetMockDbSet(_votingUsers.AsQueryable());
            _submittedVoteSet = GetMockDbSet(_submittedVotes.AsQueryable());

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
        }

        //VA-86 As a vote creator, I want others to only be able to cast one vote per account, so that we get accurate vote results


        [Test]
        public void Test_Create_Vote_For_Delayed_Vote_GetVoteById_Should_Return_Vote_With_Null_VoteAccessCode()
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
        public void Test_Create_Vote_For_Delayed_Vote_After_Vote_Start_Time_GetVoteById_Should_Return_Vote_With_Valid_VoteAccessCode()
        {
            // arrange
            EmailConfiguration emailConfig = new EmailConfiguration();
            IEmailSender emailSender = new EmailSender(emailConfig);
            ICreatedVoteRepository cvRepo = new CreatedVoteRepository(_mockContext.Object, emailSender);
            DateTime futureDate = DateTime.UtcNow.AddSeconds(5);
            CreatedVote testVote = cvRepo.AddOrUpdate(new CreatedVote
            {
                UserId = 1,
                VoteTitle = "Delayed Vote Test 2",
                VoteDiscription = "Delayed Vote Description 2",
                //DelayedVote = true,
                //VoteStartDateTime = futureDate,
                VoteCloseDateTime = futureDate.AddHours(1),
                VoteTypeId = 1,
                AnonymousVote = false
            });


            // sleep for 10 seconds - Delayed Vote Service should have created the VoteAccessCode for the test vote by that time
            Thread.Sleep(10000);

            // act
            string voteAccessCode = cvRepo.GetById(testVote.Id).VoteAccessCode; //test vote should not have a VoteAccessCode

            // assert that a five-second delayed testVote should have a VoteAccessCode, if we wait for ten seconds so the mock Delayed Vote Service
            // processes the delayed vote and generates an access code for it
            Assert.IsNotNull(voteAccessCode);
        }

        [Test]
        public void Test_Create_Vote_For_Immediate_Vote_GetDelayedVotes_Should_Return_Zero_Rows()
        {
            // arrange
            EmailConfiguration emailConfig = new EmailConfiguration();
            IEmailSender emailSender = new EmailSender(emailConfig);
            ICreatedVoteRepository cvRepo = new CreatedVoteRepository(_mockContext.Object, emailSender);
            DateTime futureDate = DateTime.UtcNow.AddDays(1);
            CreatedVote testVote = cvRepo.AddOrUpdate(new CreatedVote
            {
                UserId = 1,
                VoteTitle = "Immediate Vote Test",
                VoteDiscription = "Immediate Vote Description",
                //DelayedVote = false,
                VoteCloseDateTime = futureDate,
                VoteTypeId = 1,
                AnonymousVote = false
            });

            // act
            //List<CreatedVote> delayedVotes = cvRepo.GetDelayedVotes();

            // assert that if an immediate vote is created in test database, then GetDelayedVotes() will return zero rows
            //Assert.That(delayedVotes.Count.Equals(0));
        }

        [Test]
        public void Test_Create_Vote_For_Delayed_Vote_GetDelayedVotes_Should_Return_One_Row()
        {
            // arrange
            EmailConfiguration emailConfig = new EmailConfiguration();
            IEmailSender emailSender = new EmailSender(emailConfig);
            ICreatedVoteRepository cvRepo = new CreatedVoteRepository(_mockContext.Object, emailSender);
            DateTime futureDate = DateTime.UtcNow.AddDays(1);
            CreatedVote testVote = cvRepo.AddOrUpdate(new CreatedVote
            {
                UserId = 1,
                VoteTitle = "Delayed Vote Test 3",
                VoteDiscription = "Delayed Vote Description 3",
                //DelayedVote = true,
                //VoteStartDateTime = futureDate,
                VoteCloseDateTime = futureDate.AddHours(1),
                VoteTypeId = 1,
                AnonymousVote = false
            });

            // act
           // List<CreatedVote> delayedVotes = cvRepo.GetDelayedVotes();

            // assert that if a delayed vote is created in test database, then GetDelayedVotes() will return one row
           // Assert.That(delayedVotes.Count.Equals(1));
        }

        [Test]
        public void Test_Create_Vote_For_Delayed_Vote_GetVotesByStartDate_Should_Return_One_Row()
        {
            // arrange
            EmailConfiguration emailConfig = new EmailConfiguration();
            IEmailSender emailSender = new EmailSender(emailConfig);
            ICreatedVoteRepository cvRepo = new CreatedVoteRepository(_mockContext.Object, emailSender);
            DateTime futureDate = DateTime.UtcNow.AddDays(1);
            CreatedVote testVote = cvRepo.AddOrUpdate(new CreatedVote
            {
                UserId = 1,
                VoteTitle = "Delayed Vote Test 4",
                VoteDiscription = "Delayed Vote Description 4",
                //DelayedVote = true,
                //VoteStartDateTime = futureDate,
                VoteCloseDateTime = futureDate.AddHours(1),
                VoteTypeId = 1,
                AnonymousVote = false
            });

            // act
            //List<CreatedVote> delayedVotes = cvRepo.GetVotesByStartDate(futureDate);

            // assert that if a delayed vote is created in test database, then GetVotesByStartDate(VoteStartDateTime) will return one row
            //Assert.That(delayedVotes.Count.Equals(1));
        }
    }
}
