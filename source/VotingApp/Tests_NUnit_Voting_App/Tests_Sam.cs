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
        private Mock<DbSet<SubmittedVote>> _submittedVotesSet;
        private List<SubmittedVote> _submittedVotes;
        private Mock<DbSet<VoteAuthorizedUser>> _authorizedUsersSet;
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
                    VoteDiscription = "Mult choice description", VoteOptions = _voteOption, VoteCloseDateTime = DateTime.Now.AddDays(-5)
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
            _submittedVotes = new List<SubmittedVote>()
            {
                new SubmittedVote
                {
                    Id = 1,
                    CreatedVote = _createdVotes[1],
                    CreatedVoteId = _createdVotes[1].Id,
                    User = _votingUsers[1],
                    UserId = _votingUsers[1].Id,
                    VoteChoice = 1,
                    DateCast = DateTime.Now.AddDays(-5)
                },
                new SubmittedVote
                {
                    Id = 2,
                    CreatedVote = _createdVotes[2],
                    CreatedVoteId = _createdVotes[2].Id,
                    User = _votingUsers[1],
                    UserId = _votingUsers[1].Id,
                    VoteChoice = 3,
                    DateCast = DateTime.Now
                }
            };
            _authorizedUsers = new List<VoteAuthorizedUser>()
            {
                new VoteAuthorizedUser { CreatedVoteId = 1, Id = 1, UserName = "user1@mail.com" },
                new VoteAuthorizedUser { CreatedVoteId = 1, Id = 2, UserName = "user@mail.com" }
            };
            _voteTypesSet = GetMockDbSet(_voteTypes.AsQueryable());
            _createdVoteSet = GetMockDbSet(_createdVotes.AsQueryable());
            _voteOptionSet = GetMockDbSet(_voteOption.AsQueryable());
            _votingUsersSet = GetMockDbSet(_votingUsers.AsQueryable());
            _submittedVotesSet = GetMockDbSet(_submittedVotes.AsQueryable());
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
            _mockContext.Setup(ctx => ctx.SubmittedVotes).Returns(_submittedVotesSet.Object);
            _mockContext.Setup(ctx => ctx.Set<SubmittedVote>()).Returns(_submittedVotesSet.Object);
            _mockContext.Setup(ctx => ctx.VoteAuthorizedUsers).Returns(_authorizedUsersSet.Object);
            _mockContext.Setup(ctx => ctx.Set<VoteAuthorizedUser>()).Returns(_authorizedUsersSet.Object);
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
            _mockContext.Setup(x => x.Add(It.IsAny<SubmittedVote>())).Callback<SubmittedVote>((s) => _submittedVotes.Add(s));
            _mockContext.Setup(x => x.Update(It.IsAny<SubmittedVote>())).Callback<SubmittedVote>((s) =>
            {
                var found = _submittedVotes.FirstOrDefault(a => a.Id == s.Id);
                if (found == null)
                {
                    _submittedVotes.Add(s);
                }
                else
                {
                    var item = _submittedVotes.Where(a => a.Id == s.Id).First();
                    var index = _submittedVotes.IndexOf(item);
                    _submittedVotes[index] = s;
                }
            });
            _mockContext.Setup(x => x.Add(It.IsAny<VoteAuthorizedUser>())).Callback<VoteAuthorizedUser>((s) => _authorizedUsers.Add(s));
            _mockContext.Setup(x => x.Remove(It.IsAny<VoteAuthorizedUser>())).Callback<VoteAuthorizedUser>((s) => _authorizedUsers.Remove(s));
            /*
            _mockContext.Setup(set => set.RemoveRange(It.IsAny<VoteAuthorizedUser>()))
                .Callback<List<VoteAuthorizedUser>>((s) =>
                {
                    foreach (var user in s)
                    {
                        _authorizedUsers.Remove(user);
                    }
                });
            */

                    _mockContext.Setup(x => x.Update(It.IsAny<VoteAuthorizedUser>())).Callback<VoteAuthorizedUser>((s) =>
            {
                var found = _authorizedUsers.FirstOrDefault(a => a.Id == s.Id);
                if (found == null)
                {
                    _authorizedUsers.Add(s);
                }
                else
                {
                    var item = _authorizedUsers.Where(a => a.Id == s.Id).First();
                    var index = _authorizedUsers.IndexOf(item);
                    _authorizedUsers[index] = s;
                }
            });
            //_createdVoteSet.Verify(m => m.Add(It.IsAny<CreatedVote>()), Times.Once());
            //_mockContext.Verify(ctx => ctx.SaveChanges(), Times.Once());
        }
        //VA82
        [Test]
        public void Test_SetVoteAudio_CreatedVotePartial_Should_Set_audio()
        {
            //Arrange
            CreatedVote newCreatedVote = new CreatedVote()
            {
                Id = 99,
                VoteType = _voteTypes[0],
                AnonymousVote = false,
                UserId = 1,
                VoteTitle = "Title",
                VoteDiscription = "This is the description",
                VoteAccessCode = "abc12"
                //VoteAudioBytes = 
            };

            //Act
           // var list = voteAuthorizedUsersRepo.GetAllUsersByVoteID(1);


            //Assert that returned list of votes is sorted by date, and only by the intended user
            //Assert.True(list.Count == 2 && list[0].UserName == "user1@mail.com" && list[1].UserName == "user@mail.com");
        }

        //VA84
        [Test]
        public void Test_GetAllUserByVoteID_should_return_list_of_authorized_users()
        {
            //Arrange
            IVoteAuthorizedUsersRepo voteAuthorizedUsersRepo = new VoteAuthorizedUsersRepo(_mockContext.Object);

            //Act
            var list = voteAuthorizedUsersRepo.GetAllUsersByVoteID(1);

            
            //Assert that returned list of votes is sorted by date, and only by the intended user
            Assert.True(list.Count == 2 && list[0].UserName == "user1@mail.com" && list[1].UserName == "user@mail.com");
        }

        //VA84
        [Test] public void Test_RemoveAllByVoteID_should_remove_all_users()
        {
            //Arrange
            IVoteAuthorizedUsersRepo voteAuthorizedUsersRepo = new VoteAuthorizedUsersRepo(_mockContext.Object);

            //Act
            voteAuthorizedUsersRepo.RemoveAllByVoteID(1);
            var list = voteAuthorizedUsersRepo.GetAllUsersByVoteID(1);

            //Assert that returned list of votes is sorted by date, and only by the intended user
            Assert.True(list.Count == 0);
        }

        //VA84
        [Test]
        public void Test_GetUserById_should_return_user()
        {
            //Arrange
            IVotingUserRepositiory votingUserRepositiory = new VotingUserRepository(_mockContext.Object);

            //Act
           var user= votingUserRepositiory.GetUserById(1);

            //Assert that returned list of votes is sorted by date, and only by the intended user
            Assert.True(user != null && user.Id == 1);
        }

        //VA84
        [Test]
        public void Test_GetUserById_submitteVoteRepo_should_return_vote_with_sameID()
        {
            //Arrange
            ISubmittedVoteRepository submittedVoteRepo= new SubmittedVoteRepository(_mockContext.Object);

            //Act
            var vote = submittedVoteRepo.GetVoteById(1);

            //Assert that returned list of votes is sorted by date, and only by the intended user
            Assert.True(vote != null && vote.Id == 1);
        }

        //VA86
        [Test]
        public void Test_GetAllVotesWithNoAccessCode_should_return_votes_with_no_access_code()
        {
            //Arrange
            EmailConfiguration emailConfig = new EmailConfiguration();
            IEmailSender emailSender = new EmailSender(emailConfig);
            ICreatedVoteRepository createdVoteRepository = new CreatedVoteRepository(_mockContext.Object, emailSender);

            //Act
            var votes = createdVoteRepository.GetAllVotesWithNoAccessCode();

            //Assert that returned list of votes is sorted by date, and only by the intended user
            Assert.True(votes.Count == 2 && votes[0].Id == 2 && votes[1].Id == 3);
        }

        
        [Test]
        public void Test_VoteOptionString_should_return_string_value_of_vote_option()
        {
            //Arrange
            IVoteOptionRepository voteOptionRepository = new VoteOptionRepository(_mockContext.Object);
           
            var options = voteOptionRepository.GetVoteOptionString(3);

            //Assert that returned list of votes is sorted by date, and only by the intended user
            Assert.True(options.Count == 3);
        }

        //VA160
        [Test]
        public void Test_SubmittedVoteRepo_GetCastVotesById_should_return_list_of_sorted_submittedVotes()
        {
            //Arrange
            ISubmittedVoteRepository submittedVoteRepository = new SubmittedVoteRepository(_mockContext.Object);

            //Act
            List<SubmittedVote> subVoteList = submittedVoteRepository.GetCastVotesById(_votingUsers[1].Id);

            //Assert that returned list of votes is sorted by date, and only by the intended user
            Assert.True(subVoteList.Count == 2 && subVoteList.FirstOrDefault() == _submittedVotes[1]);
        }
        //VA160
        [Test]
        public void Test_SubmittedVoteRepo_GetCastVotesById_should_return_empty_list_if_user_has_not_cast_vote()
        {
            //Arrange
            ISubmittedVoteRepository submittedVoteRepository = new SubmittedVoteRepository(_mockContext.Object);

            //Act
            List<SubmittedVote> subVoteList = submittedVoteRepository.GetCastVotesById(1);

            Assert.That(subVoteList.Count == 0);

        }

        //VA160
        [Test]
        public void Test_SubmittedVoteRepo_GetCastVotesById_If_user_does_not_exist_return_empty_list()
        {
            //Arrange
            ISubmittedVoteRepository submittedVoteRepository = new SubmittedVoteRepository(_mockContext.Object);

            //Act
            List<SubmittedVote> subVoteList = submittedVoteRepository.GetCastVotesById(5);

            Assert.That(subVoteList.Count == 0);

        }

        //VA160
        [Test]
        public void Test_SubmittedVoteRepo_EditCastVote_changes_the_vote_returns_new_vote()
        {
            //Arrange
            ISubmittedVoteRepository submittedVoteRepository = new SubmittedVoteRepository(_mockContext.Object);
            var vote = new CreatedVote
            {
                Id = 9,
                VoteType = _voteTypes[0],
                AnonymousVote = false,
                UserId = 1,
                VoteTitle = "Title",
                VoteDiscription = "This is the description",
                VoteAccessCode = "abc123",
                VoteCloseDateTime = DateTime.Now.AddDays(1)
            };
            vote.VoteOptions = new List<VoteOption>
            {
                new VoteOption
                {
                    Id = 10,
                    CreatedVote = vote,
                    CreatedVoteId = 9,
                    VoteOptionString = "Yes"
                },
                new VoteOption
                {
                    Id = 11,
                    CreatedVote = vote,
                    CreatedVoteId = 9,
                    VoteOptionString = "No"
                },
            };
            var newSubVote = new SubmittedVote
            {
                Id = 99,
                CreatedVote = vote,
                CreatedVoteId = 9,
                UserId = 1,
                VoteChoice = 10,

            };

            EmailConfiguration emailConfig = new EmailConfiguration();
            IEmailSender emailSender = new EmailSender(emailConfig);
            var createdVoteRepo = new CreatedVoteRepository(_mockContext.Object, emailSender);
            vote = createdVoteRepo.AddOrUpdate(vote);
            _mockContext.Object.Add(newSubVote);
            //Act
            SubmittedVote editedVote = submittedVoteRepository.EditCastVote(99, 11);

            Assert.True(editedVote.VoteChoice == 11 && editedVote.Id == 99);

        }

        //VA160
        [Test]
        public void Test_SubmittedVoteRepo_EditCastVote_invalid_vote_id_should_return_null()
        {
            //Arrange
            ISubmittedVoteRepository submittedVoteRepository = new SubmittedVoteRepository(_mockContext.Object);

            //Act
            SubmittedVote editedVote = submittedVoteRepository.EditCastVote(9, 2);

            Assert.True(editedVote == null);

        }

        //VA160
        [Test]
        public void Test_SubmittedVoteRepo_EditCastVote_invalid_option_id_should_return_unedited_vote()
        {
            //Arrange
            ISubmittedVoteRepository submittedVoteRepository = new SubmittedVoteRepository(_mockContext.Object);

            //Act
            SubmittedVote editedVote = submittedVoteRepository.EditCastVote(1, 5);

            Assert.True(editedVote.VoteChoice == 1 && editedVote.Id == 1);

        }

        //VA160
        [Test]
        public void Test_SubmittedVoteRepo_EditCastVote_invalid_vote_and_option_id_should_return_null()
        {
            //Arrange
            ISubmittedVoteRepository submittedVoteRepository = new SubmittedVoteRepository(_mockContext.Object);

            //Act
            SubmittedVote editedVote = submittedVoteRepository.EditCastVote(9, 5);

            Assert.True(editedVote == null);

        }
        //VA160
        [Test]
        public void Test_SubmittedVoteRepo_EditCastVote_VotingWindowClosed_should_return_undited_submitted_vote()
        {
            //Arrange
            ISubmittedVoteRepository submittedVoteRepository = new SubmittedVoteRepository(_mockContext.Object);

            //Act
            SubmittedVote editedVote = submittedVoteRepository.EditCastVote(2, 2);

            Assert.True(editedVote.VoteChoice == 3 && editedVote.Id == 2);

        }


    

    //VA85
    [Test]
        public void Test_CreationService_should_add_options_to_yes_no_addcloseTime_add_accessCode()
        {
            IVoteOptionRepository Oprepo = new VoteOptionRepository(_mockContext.Object);
            EmailConfiguration emailConfig = new EmailConfiguration();
            IEmailSender emailSender = new EmailSender(emailConfig);
            ICreatedVoteRepository Createrepo = new CreatedVoteRepository(_mockContext.Object, emailSender);
            IVoteTypeRepository Typerepo = new VoteTypeRepository(_mockContext.Object);
            VoteCreationService voteServ = new VoteCreationService(_mockContext.Object);
            CreationService service = new CreationService(Createrepo, Typerepo, voteServ, Oprepo, null);
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
            EmailConfiguration emailConfig = new EmailConfiguration();
            IEmailSender emailSender = new EmailSender(emailConfig);
            ICreatedVoteRepository Createrepo = new CreatedVoteRepository(null, emailSender);
            IVoteTypeRepository Typerepo = new VoteTypeRepository(_mockContext.Object);
            VoteCreationService voteServ = new VoteCreationService(_mockContext.Object);
            CreationService service = new CreationService(Createrepo, Typerepo, voteServ, Oprepo, null);
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
            EmailConfiguration emailConfig = new EmailConfiguration();
            IEmailSender emailSender = new EmailSender(emailConfig);
            ICreatedVoteRepository Createrepo = new CreatedVoteRepository(_mockContext.Object, emailSender);
            IVoteTypeRepository Typerepo = new VoteTypeRepository(_mockContext.Object);
            VoteCreationService voteServ = new VoteCreationService(_mockContext.Object);
            CreationService service = new CreationService(Createrepo, Typerepo, voteServ, Oprepo,null);
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
            EmailConfiguration emailConfig = new EmailConfiguration();
            IEmailSender emailSender = new EmailSender(emailConfig);
            ICreatedVoteRepository Createrepo = new CreatedVoteRepository(_mockContext.Object, emailSender);
            IVoteTypeRepository Typerepo = new VoteTypeRepository(_mockContext.Object);
            VoteCreationService voteServ = new VoteCreationService(_mockContext.Object);
            CreationService service = new CreationService(Createrepo, Typerepo, voteServ, Oprepo, null);
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
            EmailConfiguration emailConfig = new EmailConfiguration();
            IEmailSender emailSender = new EmailSender(emailConfig);
            ICreatedVoteRepository Createrepo = new CreatedVoteRepository(null, emailSender);
            IVoteTypeRepository Typerepo = new VoteTypeRepository(_mockContext.Object);
            VoteCreationService voteServ = new VoteCreationService(_mockContext.Object);
            CreationService service = new CreationService(Createrepo, Typerepo, voteServ, Oprepo,null);
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