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
    public class RepoTests
    {
        private Mock<VotingAppDbContext> _mockContext;
        private Mock<DbSet<CreatedVote>> _createdVoteSet;
        private Mock<DbSet<VoteType>> _voteTypesSet;
        private List<CreatedVote> _createdVotes;
        private List<VoteType> _voteTypes;

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
                new VoteType { Id = 1, VoteTypeDescription = "yes/no" }
            };
            _createdVotes = new List<CreatedVote>()
            {
                new CreatedVote { Id = 1, VoteType = _voteTypes[0], Anonymous = false, UserId = null, VoteDiscription="This is the description"},
                new CreatedVote { Id = 2, VoteType = _voteTypes[0], Anonymous = false, UserId = null, VoteDiscription=null}
            };
            _voteTypesSet = GetMockDbSet(_voteTypes.AsQueryable());
            _createdVoteSet = GetMockDbSet(_createdVotes.AsQueryable());
            _mockContext = new Mock<VotingAppDbContext>();
            _mockContext.Setup(ctx => ctx.VoteTypes).Returns(_voteTypesSet.Object);
            _mockContext.Setup(ctx => ctx.Set<VoteType>()).Returns(_voteTypesSet.Object);
            _mockContext.Setup(ctx => ctx.CreatedVotes).Returns(_createdVoteSet.Object);
            _mockContext.Setup(ctx => ctx.Set<CreatedVote>()).Returns(_createdVoteSet.Object);
        }

        [Test]
        public void Test_CreatedVoteRepo_GetVoteDescription_Should_Return_Description()
        {
            ICreatedVoteRepository repo = new CreatedVoteRepository(_mockContext.Object);
            var result = repo.GetVoteDescription(1);
            Assert.AreEqual(result, "This is the description");
        }

        [Test]
        public void Test_CreatedVoteRepo_GetVoteDescription_Should_Return_Null()
        {
            ICreatedVoteRepository repo = new CreatedVoteRepository(_mockContext.Object);
            var result = repo.GetVoteDescription(2);
            Assert.AreEqual(result, null);
        }
        [Test]
        public void Test_CreatedVoteRepo_GetVoteDescription_Should_Throw_Exception()
        {
            ICreatedVoteRepository repo = new CreatedVoteRepository(_mockContext.Object);
            var result = repo.GetVoteDescription(3);
            Assert.AreEqual(result, null);
        }

        [Test]
        public void Test_CreatedVoteRepo_SetAnonymous_Should_Set_To_True()
        {
            ICreatedVoteRepository repo = new CreatedVoteRepository(_mockContext.Object);
            var result = repo.SetAnonymous();
            Assert.AreEqual(result, null);
        }
    }
}