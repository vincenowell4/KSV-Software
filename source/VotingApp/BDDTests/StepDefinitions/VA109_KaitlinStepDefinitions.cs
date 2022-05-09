using System;
using TechTalk.SpecFlow;
using BDDTests.PageObjects;
using BDDTests;
using SpecFlow.Actions.Selenium;
using OpenQA.Selenium;

namespace BDDTests.StepDefinitions
{
    [Binding]
    public class VA109_KaitlinStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly HomePage _homePage;
        private readonly AccessPage _accessPage;
        private readonly ResultsPage _resultsPage;
        private readonly IBrowserInteractions _browserInteractions;
        public VA109_KaitlinStepDefinitions(ScenarioContext context, HomePage homePage, AccessPage accessPage, ResultsPage resultsPage, IBrowserInteractions browserInteractions)
        {
            _scenarioContext = context;
            _homePage = homePage;
            _accessPage = accessPage;
            _resultsPage = resultsPage;
            _browserInteractions = browserInteractions;
        }

        [Given(@"I am on the '([^']*)' a vote page")]
        public void GivenIAmOnTheAVotePage(string access)
        {
            _accessPage.Goto(access);
        }


        [Then(@"I will see an input box for entering an access code")]
        public void ThenIWillSeeAnInputBoxForEnteringAnAccessCode()
        {
            var check = _accessPage.GetVoteResultsInputBox;
            check.Should().BeTrue();
        }

        [Then(@"I will see another input box for entering an access code")]
        public void ThenIWillSeeAnotherInputBoxForEnteringAnAccessCode()
        {
            var check = _accessPage.GetSubmitVoteInputBox;
            check.Should().BeTrue();
        }

        [Given(@"I have entered the '([^']*)' on the '([^']*)' page")]
        public void GivenIHaveEnteredTheOnThePage(string p0, string access)
        {
            _accessPage.Goto();
            _accessPage.EnterVoteResultsAccessCode(p0);
        }

        [When(@"I am on the '([^']*)' page")]
        public void WhenIAmOnThePage(string results)
        {
            _accessPage.ClickResultsSubmit();
        }

        [When(@"I click on the back to access a vote button")]
        public void WhenIClickOnTheBackToAccessAVoteButton()
        {
            _resultsPage.ClickBackToAccess();
        }

        [Then(@"I am brought back to the access a vote page")]
        public void ThenIAmBroughtBackToTheAccessAVotePage()
        {
            _browserInteractions.GetUrl().Should().Be("https://localhost:7297/Access?"); 
        }

        [Then(@"I am brought to the correct results page")]
        public void ThenIAmBroughtToTheCorrectResultsPage()
        {
            _accessPage.ClickResultsSubmit();
            _browserInteractions.GetUrl().Should().Be("https://localhost:7297/Access/Results?code=16ba88");
        }

        [Given(@"I have entered the incorrect '([^']*)' to access vote results")]
        public void GivenIHaveEnteredTheIncorrectToAccessVoteResults(string p0)
        {
            _accessPage.EnterVoteResultsAccessCode(p0);
            _accessPage.ClickResultsSubmit();
        }

        [Then(@"I will still be on the '([^']*)' page")]
        public void ThenIWillStillBeOnThePage(string access)
        {
            _browserInteractions.GetUrl().Should().Be("https://localhost:7297/Access/Results?code=123456");
        }

    }
}
