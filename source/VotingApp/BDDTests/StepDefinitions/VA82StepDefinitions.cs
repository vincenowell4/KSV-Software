using System;
using BDDTests.PageObjects;
using OpenQA.Selenium;
using SpecFlow.Actions.Selenium;
using TechTalk.SpecFlow;

namespace BDDTests.StepDefinitions
{
    [Binding]
    public class VA82StepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly HomePage _homePage;
        private readonly AccessPage _accessPage;
        private readonly ResultsPage _resultsPage;
        private readonly VoteReviewPage _voteReviewPage;
        private readonly IBrowserInteractions _browserInteractions;
        public VA82StepDefinitions(ScenarioContext context, HomePage homePage, AccessPage accessPage, ResultsPage resultsPage, VoteReviewPage voteReviewPage, IBrowserInteractions browserInteractions)
        {
            _scenarioContext = context;
            _homePage = homePage;
            _accessPage = accessPage;
            _resultsPage = resultsPage;
            _browserInteractions = browserInteractions;
            _voteReviewPage = voteReviewPage;

        }
        [When(@"I enter in the '([^']*)'")]
        public void WhenIEnterInThe(string p0)
        {
            _accessPage.EnterVoteAccessCode(p0);
        }
        [Then(@"I will be navigate to the Submit a vote page for '([^']*)'")]
        public void ThenIWillBeNavigateToTheSubmitAVotePageFor(string p0)
        {
            _browserInteractions.GetUrl().Should().Be($"https://localhost:7297/Access/Access?code={p0}");
        }

        [When(@"I navigate to the '([^']*)' page")]
        public void WhenINavigateToThePage(string voteReview)
        {
            _browserInteractions.GoToUrl("https://localhost:7297/Create/CreatedVotesReview");
        }

        [Then(@"I will see an option to play audio for the vote")]
        public void ThenIWillSeeAnOptionToPlayAudioForTheVote()
        {
            var element = _browserInteractions.WaitAndReturnElement(By.Id("VoteAudio"));
            element.Should().NotBeNull();
        }
        [Then(@"I will see audio available for each vote")]
        public void ThenIWillSeeAudioAvailableForEachVote()
        {
            var element = _browserInteractions.WaitAndReturnElement(By.Id("VoteAudio"));
            element.Should().NotBeNull();
        }

        [When(@"I navigate to the Vote History page")]
        public void WhenINavigateToTheVoteHistoryPage()
        {
            _browserInteractions.GoToUrl("https://localhost:7297/Access/VoteHistory");
        }


    }
}
