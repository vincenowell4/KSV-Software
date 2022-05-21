using BDDTests.PageObjects;
using SpecFlow.Actions.Selenium;
using System;
using TechTalk.SpecFlow;

namespace BDDTests.StepDefinitions
{
    [Binding]
    public class VA268_KaitlinStepDefinitions
    {
        private readonly HelpPage _helpPage;
        private readonly IBrowserInteractions _browserInteractions;

        public VA268_KaitlinStepDefinitions(HelpPage helpPage, IBrowserInteractions browserInteractions)
        {
            _helpPage = helpPage;
            _browserInteractions = browserInteractions;
        }

        [Given(@"I am on the help page")]
        public void GivenIAmOnTheHelpPage()
        {
            _helpPage.Goto();
        }


        [Then(@"I should see a multi-round vote accordian")]
        public void ThenIShouldSeeAMulti_RoundVoteAccordian()
        {
            var check = _helpPage.GetPollTypeAccordian;
            check.Should().BeTrue();
        }

        [Given(@"I click on the multi-round vote accordian")]
        public void GivenIClickOnTheMulti_RoundVoteAccordian()
        {
            Thread.Sleep(500);
            _helpPage.HitMultiAccordian();
            Thread.Sleep(500);
        }

        [Then(@"I should see the directions for a multi-round vote")]
        public void ThenIShouldSeeTheDirectionsForAMulti_RoundVote()
        {
            var check = _helpPage.GetMultiRoundDescription;
            check.Should().BeTrue();
        }

        [Given(@"I click on the create a poll button")]
        public void GivenIClickOnTheCreateAPollButton()
        {
           _helpPage.HitCreateButton();
        }

        [Then(@"I should be redirected to the Create page")]
        public void ThenIShouldBeRedirectedToTheCreatePage()
        {
            _browserInteractions.GetUrl().Should().Be($"https://localhost:7297/Create");
        }

        [Then(@"I should see a FAQ title")]
        public void ThenIShouldSeeAFAQTitle()
        {
            var check = _helpPage.GetFAQTitle;
            check.Should().BeTrue();
        }

        [Then(@"I should different cards with FAQ")]
        public void ThenIShouldDifferentCardsWithFAQ()
        {
            var check = _helpPage.GetFAQCards;
            check.Should().BeTrue();
        }
    }
}
