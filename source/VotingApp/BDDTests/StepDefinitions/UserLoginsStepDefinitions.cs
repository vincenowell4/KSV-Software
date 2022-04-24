using TechTalk.SpecFlow.Assist;
using BDDTests.PageObjects;
using BDDTests;

namespace BDDTests.StepDefinitions
{
#nullable disable warnings

    // Wrapper for the data we get for each user
    public class TestUser
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
    }

    [Binding]
    public class UserLoginsStepDefinitions
    {
        // The context is shared between all step definition files.
        // This is where we put data that is shared between steps in different files.
        private readonly ScenarioContext _scenarioContext;
        private readonly LoginPage _loginPage;
        private readonly HomePage _homePage;

        public UserLoginsStepDefinitions(ScenarioContext context, LoginPage loginPage, HomePage homePage)
        {
            _scenarioContext = context;
            _loginPage = loginPage;
            _homePage = homePage;
        }

        [Given(@"the following users exist")]
        public void GivenTheFollowingUsersExist(Table table)
        {
            // Nothing to do for this step other than to save the background data
            // that defines the users
            IEnumerable<TestUser> users = table.CreateSet<TestUser>();
            _scenarioContext["Users"] = users;
        }

        [Given(@"the following users do not exist")]
        public void GivenTheFollowingUsersDoNotExist(Table table)
        {
            // Same with this one, just setting up the background data
            IEnumerable<TestUser> nonUsers = table.CreateSet<TestUser>();
            _scenarioContext["NonUsers"] = nonUsers;
        }

        [Given(@"I am a user with first name '([^']*)'")]
        public void GivenIAmAUserWithFirstName(string firstName)
        {
            // Find this user, first look in users, then in non-users
            IEnumerable<TestUser> users = (IEnumerable<TestUser>)_scenarioContext["Users"];
            TestUser u = users.Where(u => u.FirstName == firstName).FirstOrDefault();
            if (u == null)
            {
                // must have been selecting from non-users
                IEnumerable<TestUser> nonUsers = (IEnumerable<TestUser>)_scenarioContext["NonUsers"];
                u = nonUsers.Where(u => u.FirstName == firstName).FirstOrDefault();
            }
            _scenarioContext["CurrentUser"] = u;
        }

        [Given(@"I am a user")]
        public void GivenIAmAUser()
        {
            // Choose one particular user.  Don't do this randomly as want our tests to be reproduceable
            TestUser u = ((IEnumerable<TestUser>)_scenarioContext["Users"]).First();
            _scenarioContext["CurrentUser"] = u;
        }

        [When(@"I login")]
        [Given(@"I login")]
        public void WhenILogin()
        {
            // Go to the login page
            _loginPage.Goto();

            // Now (attempt to) log them in.  Assumes previous steps defined the user
            TestUser u = (TestUser)_scenarioContext["CurrentUser"];
            _loginPage.EnterEmail(u.Email);
            _loginPage.EnterPassword(u.Password);
            _loginPage.Login();
        }
        
        [Then(@"I am redirected to the '([^']*)' page")]
        public void ThenIAmRedirectedToThePage(string pageName)
        {
            _homePage.GetCurrentUrl().Should().Be(Common.UrlFor(pageName));
        }

        [Then(@"I can see a personalized message in the navbar that includes my email")]
        public void ThenICanSeeAPersonalizedMessageInTheNavbarThatIncludesMyEmail()
        {
            // This is after a redirection to the homepage so we need to use that page
            TestUser u = (TestUser)_scenarioContext["CurrentUser"];
            _homePage.GetWelcomeText.Should().ContainEquivalentOf(u.Email, AtLeast.Once());
        }

        [Then(@"I can see a login error message")]
        public void ThenICanSeeALoginErrorMessage()
        {
            _loginPage.HasLoginError().Should().BeTrue();
        }
    }
}