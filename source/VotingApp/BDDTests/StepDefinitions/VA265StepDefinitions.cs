using System;
using TechTalk.SpecFlow;
using BDDTests.PageObjects;
using SpecFlow.Actions.Selenium;

namespace BDDTests.StepDefinitions
{
    [Binding]
    public class VA265StepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly SubmitVotePage _submitVotePage;


        public VA265StepDefinitions(ScenarioContext context, SubmitVotePage submitVotePage)
        {
            _scenarioContext = context;
            _submitVotePage = submitVotePage;
        }
        [Then(@"I will see a message saying that I have already cast a vote")]
        public void ThenIWillSeeAMessageSayingThatIHaveAlreadyCastAVote()
        {
            _submitVotePage.GetUpdateMessage().Should().Be("You have already submitted your vote. Resubmitting a vote will overwrite your previous vote");
        }
    }
}
