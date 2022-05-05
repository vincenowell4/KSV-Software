using System;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using EmailService;
using VotingApp.DAL.Abstract;
using VotingApp.DAL.Concrete;
using VotingApp.Models;

namespace Tests_NUnit_Voting_App
{
    public class RepoTests_Kaitlin
    {
        private Mock<VotingAppDbContext> _mockContext;
        private Mock<DbSet<CreatedVote>> _createdVoteSet;
        private Mock<DbSet<VoteType>> _voteTypesSet;
        private Mock<DbSet<VoteOption>> _voteOptionSet;
        private Mock<DbSet<SubmittedVote>> _submittedVoteSet;
        private Mock<DbSet<VotingUser>> _votingUsersSet;
        private Mock<DbSet<VoteAuthorizedUser>> _authorizedUsersSet;
        private List<CreatedVote> _createdVotes;
        private List<VoteType> _voteTypes;
        private List<VoteOption> _voteOption;
        private List<SubmittedVote> _submittedVotes;
        private List<VotingUser> _votingUsers;
        private List<VoteAuthorizedUser> _authorizedUsers;


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
                new CreatedVote { Id = 3, VoteType = _voteTypes[2], AnonymousVote = false, UserId = 1, VoteTitle = "Mult Choice Vote", VoteDiscription="Mult choice description", VoteOptions = _voteOption, TimeZone = new VoteTimeZone {Id = 1, TimeName = "Alaskan Standard Time"}},
                new CreatedVote { Id = 4, VoteType = _voteTypes[0], AnonymousVote = false, UserId= 1, VoteTitle = "Title", VoteDiscription="This is the description", VoteAccessCode = "123abc", PrivateVote = true, VoteAuthorizedUsers = _authorizedUsers},
                new CreatedVote { Id = 5, VoteType = _voteTypes[0], AnonymousVote = false, UserId= 2, VoteTitle = "open vote 1", VoteDiscription="description", VoteCloseDateTime = DateTime.Today.AddDays(5)},
                new CreatedVote { Id = 6, VoteType = _voteTypes[0], AnonymousVote = false, UserId= 2, VoteTitle = "open vote 2", VoteDiscription="descript", VoteCloseDateTime = DateTime.Today.AddDays(1)},
                new CreatedVote { Id = 7, VoteType = _voteTypes[0], AnonymousVote = false, UserId= 2, VoteTitle = "closed vote 1", VoteDiscription="des", VoteCloseDateTime = DateTime.Today.AddDays(-2)},
                new CreatedVote { Id = 8, VoteType = _voteTypes[0], AnonymousVote = false, UserId= 2, VoteTitle = "closed vote 2", VoteDiscription="desc", VoteCloseDateTime = DateTime.Today.AddDays(-3)}
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
                new SubmittedVote { CreatedVote = _createdVotes[2], CreatedVoteId = 3, Id = 1, VoteChoice = 1},
                new SubmittedVote { CreatedVote = _createdVotes[2], CreatedVoteId = 3, Id = 2, VoteChoice = 2},
                new SubmittedVote { CreatedVote = _createdVotes[2], CreatedVoteId = 3, Id = 3, VoteChoice = 2, User = _votingUsers[1]}
            };

            _authorizedUsers = new List<VoteAuthorizedUser>()
            {
                new VoteAuthorizedUser { CreatedVoteId = 4, Id = 1, UserName = "user1@mail.com" },
                new VoteAuthorizedUser { CreatedVoteId = 4, Id = 2, UserName = "user2@mail.com" },
                new VoteAuthorizedUser { CreatedVoteId = 4, Id = 3, UserName = "user3@mail.com" }
            };

            _voteTypesSet = GetMockDbSet(_voteTypes.AsQueryable());
            _createdVoteSet = GetMockDbSet(_createdVotes.AsQueryable());
            _voteOptionSet = GetMockDbSet(_voteOption.AsQueryable());
            _votingUsersSet = GetMockDbSet(_votingUsers.AsQueryable());
            _submittedVoteSet = GetMockDbSet(_submittedVotes.AsQueryable());
            _authorizedUsersSet = GetMockDbSet(_authorizedUsers.AsQueryable());

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
            _mockContext.Setup(ctx => ctx.VoteAuthorizedUsers).Returns(_authorizedUsersSet.Object);
            _mockContext.Setup(ctx => ctx.Set<VoteAuthorizedUser>()).Returns(_authorizedUsersSet.Object);
        }

        [Test]
        //VA207
        public void CreatedVoteRepo_GetClosedCreatedVotes_ShouldBeInDescendingCloseDateOrder()
        {
            EmailConfiguration emailConfig = new EmailConfiguration();
            IEmailSender emailSender = new EmailSender(emailConfig);
            ICreatedVoteRepository repo = new CreatedVoteRepository(_mockContext.Object, emailSender);

            var votes = repo.GetAllForUserId(2);
            var closedVotes = repo.GetClosedCreatedVotes(votes);

            Assert.IsTrue(closedVotes[0].VoteCloseDateTime > closedVotes[1].VoteCloseDateTime);
        }


        [Test]
        //VA207
        public void CreatedVoteRepo_GetClosedCreatedVotes_ShouldReturn2ClosedVotes()
        {
            EmailConfiguration emailConfig = new EmailConfiguration();
            IEmailSender emailSender = new EmailSender(emailConfig);
            ICreatedVoteRepository repo = new CreatedVoteRepository(_mockContext.Object, emailSender);

            var votes = repo.GetAllForUserId(2);
            var closedVotes = repo.GetClosedCreatedVotes(votes);

            Assert.AreEqual(closedVotes.Count, 2);
        }


        [Test]
        //VA207
        public void CreatedVoteRepo_GetOpenCreatedVotes_ShouldReturn2OpenVotes()
        {
            EmailConfiguration emailConfig = new EmailConfiguration();
            IEmailSender emailSender = new EmailSender(emailConfig);
            ICreatedVoteRepository repo = new CreatedVoteRepository(_mockContext.Object, emailSender);

            var votes = repo.GetAllForUserId(2);
            var openVotes = repo.GetOpenCreatedVotes(votes);

            Assert.AreEqual(openVotes.Count, 2);
        }

        [Test]
        //VA207
        public void CreatedVoteRepo_GetOpenCreatedVotes_ShouldBeInAscendingCloseDateOrder()
        {
            EmailConfiguration emailConfig = new EmailConfiguration();
            IEmailSender emailSender = new EmailSender(emailConfig);
            ICreatedVoteRepository repo = new CreatedVoteRepository(_mockContext.Object, emailSender);

            var votes = repo.GetAllForUserId(2);
            var openVotes = repo.GetOpenCreatedVotes(votes);

            Assert.IsTrue(openVotes[0].VoteCloseDateTime < openVotes[1].VoteCloseDateTime);
        }

        [Test]
        //VA206
        public void CreatedVoteRepo_SendEmails_ShouldHave3AuthorizedUsers()
        {
            EmailConfiguration emailConfig = new EmailConfiguration();
            IEmailSender emailSender = new EmailSender(emailConfig);
            ICreatedVoteRepository repo = new CreatedVoteRepository(_mockContext.Object, emailSender);
            IVoteAuthorizedUsersRepo voteAuthRepo = new VoteAuthorizedUsersRepo(_mockContext.Object);
            var authUsers = voteAuthRepo.GetAllUsersByVoteID(4);

            Assert.AreEqual(authUsers.Count, 3);
            //repo.SendEmails(authUsers, _createdVotes[3], _createdVotes[3].VoteAccessCode);
        }

        [Test]
        //VA206
        public void CreatedVoteRepo_SendEmails_ShouldHaveCorrectAccessCode()
        {
            EmailConfiguration emailConfig = new EmailConfiguration();
            IEmailSender emailSender = new EmailSender(emailConfig);
            ICreatedVoteRepository repo = new CreatedVoteRepository(_mockContext.Object, emailSender);

            Assert.AreEqual(_createdVotes[3].VoteAccessCode, "123abc");
            //repo.SendEmails(authUsers, _createdVotes[3], _createdVotes[3].VoteAccessCode);
        }


        [Test]
        //VA83
        public void SubmittedVoteRepo_MatchingOrderOptionsList_ShouldBeCorrectOrderOptions()
        {
            ISubmittedVoteRepository repo = new SubmittedVoteRepository(_mockContext.Object);
            var check = repo.MatchingOrderOptionsList(_createdVotes[2].Id, _voteOption);
            Assert.IsTrue(check[0] == "option 1");
            Assert.IsTrue(check[1] == "option 2");
        }

        [Test]
        //VA83
        public void SubmittedVoteRepo_MatchingOrderOptionsList_ShouldHave2Options()
        {
            ISubmittedVoteRepository repo = new SubmittedVoteRepository(_mockContext.Object);
            var check = repo.MatchingOrderOptionsList(_createdVotes[2].Id, _voteOption);
            Assert.AreEqual(check.Count(), 2);
        }

        [Test]
        //VA83
        public void SubmittedVoteRepo_TotalVotesPerOption_ShouldReturnCount2Options()
        {
            ISubmittedVoteRepository repo = new SubmittedVoteRepository(_mockContext.Object);
            var check = repo.TotalVotesPerOption(_createdVotes[2].Id, _voteOption);
            Assert.AreEqual(check.Count(), 2);
        }

        [Test]
        //VA83
        public void SubmittedVoteRepo_TotalVotesPerOption_ShouldBeInCorrectOrder()
        {
            ISubmittedVoteRepository repo = new SubmittedVoteRepository(_mockContext.Object);
            var check = repo.TotalVotesPerOption(_createdVotes[2].Id, _voteOption);
            Assert.IsTrue(check[0] == 1);
            Assert.IsTrue(check[1] == 2);
        }

        [Test]
        //VA80
        public void SubmittedVoteRepo_TotalVotesForEachOption_ShouldReturnCount3Options()
        {
            ISubmittedVoteRepository repo = new SubmittedVoteRepository(_mockContext.Object);
            var check = repo.TotalVotesForEachOption(_createdVotes[2].Id, _voteOption);
            Assert.AreEqual(check.Count(), 3);
        }

        [Test]
        //VA80
        public void SubmittedVoteRepo_TotalVotesForEachOption_Option1ShouldHave1Vote()
        {
            ISubmittedVoteRepository repo = new SubmittedVoteRepository(_mockContext.Object);
            var check = repo.TotalVotesForEachOption(_createdVotes[2].Id, _voteOption);
            var checks = false; 
            foreach (var vote in check)
            {
                if (vote.Key.Id == 1 && vote.Value == 1)
                {
                    checks = true;
                }
            }

            Assert.IsTrue(checks);
        }


        [Test]
        //VA80
        public void SubmittedVoteRepo_TotalVotesForEachOption_Option2ShouldHave2Votes()
        {
            ISubmittedVoteRepository repo = new SubmittedVoteRepository(_mockContext.Object);
            var check = repo.TotalVotesForEachOption(_createdVotes[2].Id, _voteOption);
            var checks = false;
            foreach (var vote in check)
            {
                if (vote.Key.Id == 2 && vote.Value == 2)
                {
                    checks = true;
                }
            }

            Assert.IsTrue(checks);
        }

        [Test]
        //VA80
        public void SubmittedVoteRepo_TotalVotesForEachOption_Option3ShouldHave0Votes()
        {
            ISubmittedVoteRepository repo = new SubmittedVoteRepository(_mockContext.Object);
            var check = repo.TotalVotesForEachOption(_createdVotes[2].Id, _voteOption);
            var checks = false;
            foreach (var vote in check)
            {
                if (vote.Key.Id == 3 && vote.Value == 0)
                {
                    checks = true;
                }
            }

            Assert.IsTrue(checks);
        }

        [Test]
        //VA80
        public void SubmittedVoteRepo_TotalVotesForEachOption_ShouldThrowNullException()
        {
            ISubmittedVoteRepository repo = new SubmittedVoteRepository(_mockContext.Object);
            _voteOption = null;
            Assert.Throws<NullReferenceException>(() => repo.TotalVotesForEachOption(_createdVotes[2].Id, _voteOption));
        }

        [Test]
        //VA81
        public void SubmittedVoteRepo_GetAllSubmittedVotesWithLoggedInUsers_ShouldReturn1()
        {
            ISubmittedVoteRepository repo = new SubmittedVoteRepository(_mockContext.Object);
            var check = repo.GetAllSubmittedVotesWithLoggedInUsers(_createdVotes[2].Id, _voteOption);
            Assert.IsTrue(check.Count() == 1);
        }

        //[Test]
        //VA81
        //public void SubmittedVoteRepo_GetAllSubmittedVotesWithLoggedInUsers_ShouldGetCorrectVoteOptionString()
        //{
        //    ISubmittedVoteRepository repo = new SubmittedVoteRepository(_mockContext.Object);
        //    var check = repo.GetAllSubmittedVotesWithLoggedInUsers(_createdVotes[2].Id, _voteOption);
        //    var item = check.ElementAt(0);

        //    Assert.AreEqual(item.Key.VoteOptionString, "option 2");
        //}

        [Test]
        //VA81
        public void SubmittedVoteRepo_GetAllSubmittedVotesForUsersNotLoggedIn_ShouldGetCorrectVoteOptionString()
        {
            ISubmittedVoteRepository repo = new SubmittedVoteRepository(_mockContext.Object);
            var check = repo.GetAllSubmittedVotesForUsersNotLoggedIn(_createdVotes[2].Id, _voteOption);
            var item = check.ElementAt(0);
            var item2 = check.ElementAt(1);

            Assert.AreEqual(item.Key.VoteOptionString, "option 1");
            Assert.AreEqual(item2.Key.VoteOptionString, "option 2");
        }

        [Test]
        //VA81
        public void SubmittedVoteRepo_GetAllSubmittedVotesForUsersNotLoggedIn_ShouldReturn2()
        {
            ISubmittedVoteRepository repo = new SubmittedVoteRepository(_mockContext.Object);
            var check = repo.GetAllSubmittedVotesForUsersNotLoggedIn(_createdVotes[2].Id, _voteOption);
            Assert.IsTrue(check.Count() == 2);
        }

        [Test]
        //VA81
        public void SubmittedVoteRepo_GetTotalSubmittedVotes_ShouldReturn3()
        {
            ISubmittedVoteRepository repo = new SubmittedVoteRepository(_mockContext.Object);
            var check = repo.GetTotalSubmittedVotes(_createdVotes[2].Id);
            Assert.AreEqual(check, 3);
        }

        [Test]
        //VA81
        public void SubmittedVoteRepo_GetWinner_ShouldReturn1WinnerTotal()
        {
            ISubmittedVoteRepository repo = new SubmittedVoteRepository(_mockContext.Object);
            var submittedVotes = repo.TotalVotesForEachOption(_createdVotes[2].Id, _voteOption);
            var check = repo.GetWinner(submittedVotes);
            Assert.AreEqual(check.Count, 1);
        }

        [Test]
        //VA81
        public void SubmittedVoteRepo_GetWinner_ShouldReturn1CorrectWinnerVoteOptionString()
        {
            ISubmittedVoteRepository repo = new SubmittedVoteRepository(_mockContext.Object);
            var submittedVotes = repo.TotalVotesForEachOption(_createdVotes[2].Id, _voteOption);
            var check = repo.GetWinner(submittedVotes);
            Assert.AreEqual(check.ElementAt(0).Key.VoteOptionString, "option 2");
        }

        [Test]
        //VA81
        public void SubmittedVoteRepo_GetWinner_ShouldThrowNullException()
        {
            ISubmittedVoteRepository repo = new SubmittedVoteRepository(_mockContext.Object);
            Dictionary<VoteOption, int>? submittedVotes = null;
            Assert.Throws<ArgumentNullException>(() => repo.GetWinner(submittedVotes));
        }
    }
}
