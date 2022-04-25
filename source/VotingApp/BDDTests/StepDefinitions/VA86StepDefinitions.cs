using TechTalk.SpecFlow.Assist;
using BDDTests.PageObjects;
using BDDTests;

namespace BDDTests.StepDefinitions
{
#nullable disable warnings

    [Binding]
    public class VA86StepDefinitions
    {
        // The context is shared between all step definition files.
        // This is where we put data that is shared between steps in different files.
        private readonly ScenarioContext _scenarioContext;
        private readonly LoginPage _loginPage;
        private readonly CreatePage _createPage;
        private readonly HomePage _homePage;

        public VA86StepDefinitions(ScenarioContext context, LoginPage loginPage, HomePage homePage, CreatePage createPage)
        {
            _scenarioContext = context;
            _loginPage = loginPage;
            _homePage = homePage;
            _createPage = createPage;
         }

        [Given(@"the following user exists")]
        public void GivenTheFollowingUsersExist(Table table)
        {
            // Nothing to do for this step other than to save the background data
            // that defines the users
            IEnumerable<TestUser> users = table.CreateSet<TestUser>();
            _scenarioContext["Users"] = users;
        }

        [Given(@"the following user does not exist")]
        public void GivenTheFollowingUsersDoNotExist(Table table)
        {
            // Same with this one, just setting up the background data
            IEnumerable<TestUser> nonUsers = table.CreateSet<TestUser>();
            _scenarioContext["NonUsers"] = nonUsers;
        }

        [Given(@"I am a user that isn't logged in")]
        public void GivenIAmAUserThatIsntLoggedIn()
        {
            _homePage.Goto();
        }

        [When(@"I navigate to '([^']*)' page")]
        public void WhenINavigateToPage(string create)
        {
            _createPage.Goto();
        }

        [Then(@"I will not see the Start Immediate button")]
        public void ThenIWillNotSeeTheStartImmediateButton()
        {
            bool isVoteNowStartDisplayed = _createPage.GetVoteStartNowDisplayed;
            isVoteNowStartDisplayed.Should().BeTrue();
        }

        [Then(@"I will not see the Start Future Vote button")]
        public void ThenIWillNotSeeTheStartFutureVoteButton()
        {
            bool isVoteNowFutureDisplayed = _createPage.GetVoteStartFutureDisplayed;
            isVoteNowFutureDisplayed.Should().BeTrue();
        }


        [Given(@"I am a user that is logged in")]
        public void GivenIAmAUserThatIsLoggedIn()
        {
            _loginPage.Goto();
            IEnumerable<TestUser> users = (IEnumerable<TestUser>)_scenarioContext["Users"];
            TestUser u = users.Where(u => u.FirstName == "Vince").FirstOrDefault();
            _scenarioContext["CurrentUser"] = u;

            _loginPage.EnterEmail(u.Email);
            _loginPage.EnterPassword(u.Password);
            _loginPage.Login();
        }

        [Then(@"I will see the Start Immediate Vote button")]
        public void ThenIWillSeeTheStartImmediateVoteButton()
        {
            bool isVoteNowStartDisplayed = _createPage.GetVoteStartNowDisplayed;
            isVoteNowStartDisplayed.Should().BeTrue();
        }

        [Then(@"I will see the Start Future Vote button")]
        public void ThenIWillSeeTheStartFutureVoteButton()
        {
            bool isVoteFutureStartDisplayed = _createPage.GetVoteStartFutureDisplayed;
            isVoteFutureStartDisplayed.Should().BeTrue();
        }

        [Then(@"the Future DateTime textbox will be enabled")]
        public void ThenTheFutureDateTimeTextboxWillBeEnabled()
        {
            bool isVoteOpenDateTimeEnabled = _createPage.GetVoteOpenDateTimeEnabled;
            isVoteOpenDateTimeEnabled.Should().BeTrue();
        }

        [When(@"I click on Start Immediate Vote")]
        public void WhenIClickOnStartImmediateVote()
        {
            _createPage.StartVoteNowClick();
        }

        [When(@"I click on Start Future Vote")]
        public void WhenIClickOnStartFutureVote()
        {
            _createPage.StartVoteFutureClick();
        }

        [When(@"I enter a Future Date")]
        public void WhenIEnterAFutureDate()
        {
            _createPage.EnterFutureDate("2112-04-01 00:00:00");
        }

        [Then(@"the Future DateTime textbox will be cleared")]
        public void ThenTheFutureDateTimeTextboxWillBeCleared()
        {
            _createPage.GetFutureDateText.Should().BeEmpty();
        }

        [Then(@"the Future DateTime textbox will be disabled")]
        public void ThenTheFutureDateTimeTextboxWillBeDisabled()
        {
            bool isVoteOpenDateTimeEnabled = _createPage.GetVoteOpenDateTimeEnabled;
            isVoteOpenDateTimeEnabled.Should().BeFalse();
        }

        //This test is for VA-59
        [Then(@"clicking Vote Types will show Multiple Choice Multi-Round Vote as a Vote Type option")]
        public void ThenClickingVoteTypesWillShowMultipleChoiceMulti_RoundVoteAsAVoteTypeOption()
        {
            var check = _createPage.SetVoteTypeDropDown;
            check.Should().NotBeNull();
        }
    }
}
