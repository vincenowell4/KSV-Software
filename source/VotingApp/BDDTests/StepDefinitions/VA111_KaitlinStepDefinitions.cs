using System;
using BDDTests.PageObjects;
using SpecFlow.Actions.Selenium;
using TechTalk.SpecFlow;

namespace BDDTests.StepDefinitions
{
    [Binding]
    public class VA111_KaitlinStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly HomePage _homePage;
        private readonly AdminPage _adminPage;
        private readonly IBrowserInteractions _browserInteractions;

        public VA111_KaitlinStepDefinitions(ScenarioContext context, HomePage homePage, AdminPage adminPage, IBrowserInteractions browserInteractions)
        {
            _scenarioContext = context;
            _homePage = homePage;
            _adminPage = adminPage;
            _browserInteractions = browserInteractions;
        }
        [When(@"I click on the logging button")]
        public void WhenIClickOnTheLoggingButton()
        {
            _adminPage.HitLogButton();
            Thread.Sleep(500);
        }


        [When(@"I go to the Admin page")]
        public void WhenIGoToTheAdminPage()
        {
            _adminPage.Goto();
        }

        [Then(@"I will see a button for logging info")]
        public void ThenIWillSeeAButtonForLoggingInfo()
        {
            var check = _adminPage.GetLoggingButton;
            check.Should().BeTrue();
        }

        [Then(@"I will see a table with logging info")]
        public void ThenIWillSeeATableWithLoggingInfo()
        {
            var check = _adminPage.GetLoggingTable;
            check.Should().BeTrue();
        }
    }
}
