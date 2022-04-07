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
    public class ClassTests_Sam
    {
        private Mock<VotingAppDbContext> _mockContext;
        private Mock<DbSet<CreatedVote>> _createdVoteSet;
        private Mock<DbSet<VoteType>> _voteTypesSet;
        private Mock<DbSet<VoteOption>> _voteOptionSet;
        private List<CreatedVote> _createdVotes;
        private List<VoteType> _voteTypes;
        private List<VoteOption> _voteOption;
        private Mock<DbSet<VotingUser>> _votingUsersSet;
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
                new VoteType {Id = 1, VotingType = "Yes/No Vote", VoteTypeDescription = "yes/no discription"},
                new VoteType {Id = 2, VotingType = null, VoteTypeDescription = "null discription"},
                new VoteType
                    {Id = 3, VotingType = "Multiple Choice Vote", VoteTypeDescription = "multiple choice description"}
            };
            _createdVotes = new List<CreatedVote>()
            {
                new CreatedVote
                {
                    Id = 1, VoteType = _voteTypes[0], AnonymousVote = false, UserId = 1, VoteTitle = "Title",
                    VoteDiscription = "This is the description", VoteAccessCode = "abc123"
                },
                new CreatedVote
                {
                    Id = 2, VoteType = _voteTypes[0], AnonymousVote = true, UserId = 1, VoteTitle = null,
                    VoteDiscription = null
                },
                new CreatedVote
                {
                    Id = 3, VoteType = _voteTypes[2], AnonymousVote = false, UserId = 1, VoteTitle = "Mult Choice Vote",
                    VoteDiscription = "Mult choice description", VoteOptions = _voteOption
                }
            };
            _voteOption = new List<VoteOption>()
            {
                new VoteOption
                    {CreatedVote = _createdVotes[2], CreatedVoteId = 3, Id = 1, VoteOptionString = "option 1"},
                new VoteOption
                    {CreatedVote = _createdVotes[2], CreatedVoteId = 3, Id = 2, VoteOptionString = "option 2"},
                new VoteOption
                    {CreatedVote = _createdVotes[2], CreatedVoteId = 3, Id = 3, VoteOptionString = "option 3"}
            };

            _votingUsers = new List<VotingUser>()
            {
                new VotingUser {Id = 1, NetUserId = "ABC123CBA31", UserName = "name@mail.com"},
                new VotingUser {Id = 2, NetUserId = "123456", UserName = "name2@mail.com"}
            };

            _voteTypesSet = GetMockDbSet(_voteTypes.AsQueryable());
            _createdVoteSet = GetMockDbSet(_createdVotes.AsQueryable());
            _voteOptionSet = GetMockDbSet(_voteOption.AsQueryable());
            _votingUsersSet = GetMockDbSet(_votingUsers.AsQueryable());

            _mockContext = new Mock<VotingAppDbContext>();
            _mockContext.Setup(ctx => ctx.VoteTypes).Returns(_voteTypesSet.Object);
            _mockContext.Setup(ctx => ctx.Set<VoteType>()).Returns(_voteTypesSet.Object);
            _mockContext.Setup(ctx => ctx.CreatedVotes).Returns(_createdVoteSet.Object);
            _mockContext.Setup(ctx => ctx.Set<CreatedVote>()).Returns(_createdVoteSet.Object);
            _mockContext.Setup(ctx => ctx.VoteOptions).Returns(_voteOptionSet.Object);
            _mockContext.Setup(ctx => ctx.Set<VoteOption>()).Returns(_voteOptionSet.Object);
            _mockContext.Setup(ctx => ctx.VotingUsers).Returns(_votingUsersSet.Object);
            _mockContext.Setup(ctx => ctx.Set<VotingUser>()).Returns(_votingUsersSet.Object);
            
        }

        [Test]
        public void Test_CreationService_Create_Should_Pass()
        {
            IVoteOptionRepository VoteOpRepo = new VoteOptionRepository(_mockContext.Object);
            ICreatedVoteRepository CreatedVoteRepo= new CreatedVoteRepository(_mockContext.Object);
            VoteCreationService voteService = new VoteCreationService(_mockContext.Object);
            IVoteTypeRepository voteTypeRepo = new VoteTypeRepository(_mockContext.Object);
            var creationService = new CreationService(CreatedVoteRepo, voteTypeRepo, voteService, VoteOpRepo);
            var newVote = new CreatedVote
            {
                Id = 99,
                VoteType = _voteTypes[0],
                AnonymousVote = false,
                UserId = 1,
                VoteTitle = "Title",
                VoteDiscription = "This is the description",
            };
            var found = CreatedVoteRepo.GetById(99);
            Assert.True(found.VoteOptions.ToList()[0].VoteOptionString == "No" &&
                        found.VoteOptions.ToList()[1].VoteOptionString == "Yes");
        }
    }

}