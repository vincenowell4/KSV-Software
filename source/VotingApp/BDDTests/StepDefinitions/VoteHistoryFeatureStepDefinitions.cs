using System;
using TechTalk.SpecFlow;
using BDDTests.PageObjects;
using BDDTests;
using SpecFlow.Actions.Selenium;
using OpenQA.Selenium;
namespace BDDTests.StepDefinitions
{
    [Binding]
    public class VoteHistoryFeatureStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly LoginPage _loginPage;
        private readonly HomePage _homePage;
        private readonly IBrowserInteractions _browserInteractions;
        public VoteHistoryFeatureStepDefinitions(ScenarioContext context, LoginPage loginPage, HomePage homePage, IBrowserInteractions browserInteractions)
        {
            _scenarioContext = context;
            _loginPage = loginPage;
            _homePage = homePage;
            _browserInteractions = browserInteractions;
        }
        [Then(@"I can see the Vote History button in the navbar")]
        public void ThenICanSeeTheVoteHistoryButtonInTheNavbar()
        {
            //TestUser u = (TestUser)_scenarioContext["CurrentUser"];
            var items = _homePage.GetUserItemsTexts();
            items.Should().Contain("Vote History");
        }
        [Given(@"I am on the '([^']*)' page")]
        public void GivenIAmOnThePage(string home)
        {
            _homePage.Goto();
        }

        [Then(@"I cannot see the Vote History Button in the navbar")]
        public void ThenICannotSeeTheVoteHistoryButtonInTheNavbar()
        {
            var items = _homePage.GetUserItemsTexts();
            items.Should().NotContain("Vote History");
        }
        [Given(@"I navigate to the Vote History page")]
        public void GivenINavigateToTheVoteHistoryPage()
        {
            _browserInteractions.GoToUrl("https://localhost:7297/Access/VoteHistory");

        }

        [Then(@"I will be redirect to the login page")]
        public void ThenIWillBeRedirectToTheLoginPage()
        {
            _browserInteractions.GetUrl().Should().Be("https://localhost:7297/Identity/Account/Login");
        }
        [Then(@"I navigate to the Vote History page")]
        public void ThenINavigateToTheVoteHistoryPage()
        {
            _browserInteractions.GoToUrl("https://localhost:7297/Access/VoteHistory");
        }

        [Then(@"I will be redirected to the Vote History Page")]
        public void ThenIWillBeRedirectedToTheVoteHistoryPage()
        {
            _browserInteractions.GetUrl().Should().Be("https://localhost:7297/Access/VoteHistory");
        }


    }
}
