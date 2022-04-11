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
        private readonly HomePage _homePage;

        public VA86StepDefinitions(ScenarioContext context, LoginPage loginPage, HomePage homePage)
        {
            _scenarioContext = context;
            _loginPage = loginPage;
            _homePage = homePage;
        }



        [Given(@"the following user exists")]
        public void GivenTheFollowingUserExists(Table table)
        {
            throw new PendingStepException();
        }

        [Given(@"the following user does not exist")]
        public void GivenTheFollowingUserDoesNotExist(Table table)
        {
            throw new PendingStepException();
        }

        [Given(@"I am a user that isn't logged in")]
        public void GivenIAmAUserThatIsntLoggedIn()
        {
            throw new PendingStepException();
        }

        [When(@"I navigate to '([^']*)' page")]
        public void WhenINavigateToPage(string create)
        {
            throw new PendingStepException();
        }

        [Then(@"I will not see the Start Immediate button")]
        public void ThenIWillNotSeeTheStartImmediateButton()
        {
            throw new PendingStepException();
        }

        [Then(@"I will not see the Start Future Vote button")]
        public void ThenIWillNotSeeTheStartFutureVoteButton()
        {
            throw new PendingStepException();
        }
    }
}
