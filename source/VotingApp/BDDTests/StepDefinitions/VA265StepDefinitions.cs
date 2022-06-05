using System;
using BDDTests.PageObjects;
using OpenQA.Selenium;
using SpecFlow.Actions.Selenium;
using TechTalk.SpecFlow;

namespace BDDTests.StepDefinitions
{
    [Binding]
    public class VA265StepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly SubmitVotePage _submitVotePage;
        private readonly IBrowserInteractions _browserInteractions;

        public VA265StepDefinitions(ScenarioContext context, SubmitVotePage submitVotePage, IBrowserInteractions browserInteractions)
        {
            _scenarioContext = context;
            _submitVotePage = submitVotePage;
            _browserInteractions = browserInteractions;
        }
        [Then(@"I will see a message saying that I have already cast a vote")]
        public void ThenIWillSeeAMessageSayingThatIHaveAlreadyCastAVote()
        {
            _submitVotePage.GetUpdateMessage().Should().Be("You have already submitted your vote. Resubmitting a vote will overwrite your previous vote");
        }
        [Then(@"I will see qr code available for each vote")]
        public void ThenIWillSeeQrCodeAvailableForEachVote()
        {
            var element = _browserInteractions.WaitAndReturnElement(By.Id("VoteQR"));
            element.Should().NotBeNull();
        }

    }
}
