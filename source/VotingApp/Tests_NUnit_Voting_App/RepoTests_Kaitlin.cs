using System;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
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
                new SubmittedVote { CreatedVote = _createdVotes[2], CreatedVoteId = 3, Id = 1, VoteChoice = 1},
                new SubmittedVote { CreatedVote = _createdVotes[2], CreatedVoteId = 3, Id = 2, VoteChoice = 2},
                new SubmittedVote { CreatedVote = _createdVotes[2], CreatedVoteId = 3, Id = 3, VoteChoice = 2}
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
    }
}
