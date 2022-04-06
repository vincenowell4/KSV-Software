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
    public class Tests_Sam
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
                new VoteType { Id = 1, VotingType = "Yes/No Vote", VoteTypeDescription = "yes/no discription" },
                new VoteType { Id = 2, VotingType = null, VoteTypeDescription = "null discription" },
                new VoteType
                    { Id = 3, VotingType = "Multiple Choice Vote", VoteTypeDescription = "multiple choice description" }
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
                    { CreatedVote = _createdVotes[2], CreatedVoteId = 3, Id = 1, VoteOptionString = "option 1" },
                new VoteOption
                    { CreatedVote = _createdVotes[2], CreatedVoteId = 3, Id = 2, VoteOptionString = "option 2" },
                new VoteOption
                    { CreatedVote = _createdVotes[2], CreatedVoteId = 3, Id = 3, VoteOptionString = "option 3" }
            };

            _votingUsers = new List<VotingUser>()
            {
                new VotingUser { Id = 1, NetUserId = "ABC123CBA31", UserName = "name@mail.com" },
                new VotingUser { Id = 2, NetUserId = "123456", UserName = "name2@mail.com" }
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
            //_createdVoteSet.Verify(m => m.Add(It.IsAny<CreatedVote>()), Times.Once());
            //_mockContext.Verify(ctx => ctx.SaveChanges(), Times.Once());
        }

        //VA85
        [Test]
        public void Test_CreationService_should_add_options_to_yes_no_addcloseTime_add_accessCode()
        {
            IVoteOptionRepository Oprepo = new VoteOptionRepository(_mockContext.Object);
            ICreatedVoteRepository Createrepo = new CreatedVoteRepository(_mockContext.Object);
            IVoteTypeRepository Typerepo = new VoteTypeRepository(_mockContext.Object);
            VoteCreationService voteServ = new VoteCreationService(_mockContext.Object);
            CreationService service = new CreationService(Createrepo, Typerepo, voteServ, Oprepo);
            var newVote = new CreatedVote
            {
                VoteTypeId = 1,
                AnonymousVote = false,
                UserId = 1,
                VoteTitle = "Title",
                VoteDiscription = "This is the description",
                
            };
            service.Create(ref newVote);
            Assert.True(newVote.VoteOptions.Count() == 2 && newVote.VoteCloseDateTime != null && newVote.VoteAccessCode != null);
        }
        //VA85
        [Test]
        public void Test_CreationService_should_return_error_message()
        {
            IVoteOptionRepository Oprepo = new VoteOptionRepository(_mockContext.Object);
            ICreatedVoteRepository Createrepo = new CreatedVoteRepository(null);
            IVoteTypeRepository Typerepo = new VoteTypeRepository(_mockContext.Object);
            VoteCreationService voteServ = new VoteCreationService(_mockContext.Object);
            CreationService service = new CreationService(Createrepo, Typerepo, voteServ, Oprepo);
            var newVote = new CreatedVote
            {
                VoteTypeId = 1,
                AnonymousVote = false,
                UserId = 1,
                VoteTitle = "Title",
                VoteDiscription = "This is the description",

            };
            var result = service.Create(ref newVote);
            Assert.True(result != null);
        }
        //VA85
        [Test]
        public void Test_CreationService_edit_should_change_type()
        {
            IVoteOptionRepository Oprepo = new VoteOptionRepository(_mockContext.Object);
            ICreatedVoteRepository Createrepo = new CreatedVoteRepository(_mockContext.Object);
            IVoteTypeRepository Typerepo = new VoteTypeRepository(_mockContext.Object);
            VoteCreationService voteServ = new VoteCreationService(_mockContext.Object);
            CreationService service = new CreationService(Createrepo, Typerepo, voteServ, Oprepo);
            var newVote = new CreatedVote
            {
                Id = 3,
                VoteTypeId = 1,
                VoteType = _voteTypes[0],
                AnonymousVote = false,
                UserId = 1,
                VoteTitle = "Title",
                VoteDiscription = "This is the description",

            };
            var result = service.Edit(ref newVote, 2);
            Assert.True(newVote.VoteOptions.Count() == 2 && newVote.VoteTypeId == 1); 
            
        }
        //VA85
        [Test]
        public void Test_CreationService_edit_should_change_type_and_remove_options()
        {
            IVoteOptionRepository Oprepo = new VoteOptionRepository(_mockContext.Object);
            ICreatedVoteRepository Createrepo = new CreatedVoteRepository(_mockContext.Object);
            IVoteTypeRepository Typerepo = new VoteTypeRepository(_mockContext.Object);
            VoteCreationService voteServ = new VoteCreationService(_mockContext.Object);
            CreationService service = new CreationService(Createrepo, Typerepo, voteServ, Oprepo);
            var newVote = new CreatedVote
            {
                Id = 3,
                VoteTypeId = 2,
                VoteType = _voteTypes[0],
                AnonymousVote = false,
                UserId = 1,
                VoteTitle = "Title",
                VoteDiscription = "This is the description",

            };
            var result = service.Edit(ref newVote, 1);
            Assert.True(newVote.VoteOptions.Count() == 0);

        }
        //VA85
        [Test]
        public void Test_CreationService_edit_should_send_error_message()
        {
            IVoteOptionRepository Oprepo = new VoteOptionRepository(_mockContext.Object);
            ICreatedVoteRepository Createrepo = new CreatedVoteRepository(null);
            IVoteTypeRepository Typerepo = new VoteTypeRepository(_mockContext.Object);
            VoteCreationService voteServ = new VoteCreationService(_mockContext.Object);
            CreationService service = new CreationService(Createrepo, Typerepo, voteServ, Oprepo);
            var newVote = new CreatedVote
            {
                Id = 3,
                VoteTypeId = 2,
                VoteType = _voteTypes[0],
                AnonymousVote = false,
                UserId = 1,
                VoteTitle = "Title",
                VoteDiscription = "This is the description",

            };
            var result = service.Edit(ref newVote, 1);
            Assert.True(result != "");

        }
        //VA_IDK_OLD_Story_I_forgot_To_Test
        [Test]
        public void Test_VoteCreationService_should_generateCode()
        {
            VoteCreationService voteServ = new VoteCreationService(_mockContext.Object);
            var newcode = voteServ.generateCode();
            Assert.True(newcode != null);
        }
    }
}