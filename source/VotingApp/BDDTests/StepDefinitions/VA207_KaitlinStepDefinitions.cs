using System;
using TechTalk.SpecFlow;
using BDDTests.PageObjects;
using BDDTests;
using SpecFlow.Actions.Selenium;
using OpenQA.Selenium;

namespace BDDTests.StepDefinitions
{
    [Binding]
    public class VA207_KaitlinStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly CreatePage _createPage;
        private readonly IBrowserInteractions _browserInteractions;

        public VA207_KaitlinStepDefinitions(ScenarioContext context, CreatePage createPage, IBrowserInteractions browserInteractions)
        {
            _scenarioContext = context;
            _createPage = createPage;
            _browserInteractions = browserInteractions;
        }

        [Given(@"I am on the create a vote page")]
        public void GivenIAmOnTheCreateAVotePage()
        {
            _createPage.Goto();
        }


        [Then(@"I will see a collapsible for vote type descriptions")]
        public void ThenIWillSeeACollapsibleForVoteTypeDescriptions()
        {
            var check = _createPage.GetVoteTypeCollapsibleDisplayed;
            check.Should().BeTrue();
        }

        [Then(@"I will see one area with a header labeled required")]
        public void ThenIWillSeeOneAreaWithAHeaderLabeledRequired()
        {
            var check = _createPage.GetVoteRequiredHeader;
            check.Should().BeTrue();
        }

        [Then(@"it will contain a vote title field")]
        public void ThenItWillContainAVoteTitleField()
        {
            var check = _createPage.GetVoteTitleArea;
            check.Should().BeTrue();
        }

        [Then(@"it will contain a vote description field")]
        public void ThenItWillContainAVoteDescriptionField()
        {
            var check = _createPage.GetVoteDescriptionArea;
            check.Should().BeTrue();
        }

        [Then(@"it will contain a vote type field")]
        public void ThenItWillContainAVoteTypeField()
        {
            var check = _createPage.GetVoteTypeDropDown;
            check.Should().BeTrue();
        }

    }
}
