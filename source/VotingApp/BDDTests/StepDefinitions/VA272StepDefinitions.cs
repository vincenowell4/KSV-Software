using BDDTests.PageObjects;
using System;
using TechTalk.SpecFlow;

namespace BDDTests.StepDefinitions
{
    [Binding]
    public class VA272StepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly AboutPage _aboutPage;

        public VA272StepDefinitions(ScenarioContext context, AboutPage aboutPage)
        {
            _scenarioContext = context;
            _aboutPage = aboutPage;
        }


        [Given(@"I click on the About link")]
        public void GivenIClickOnTheAboutLink()
        {
            _aboutPage.Goto();
        }

         [Then(@"I will see the page title About Opiniony")]
        public void ThenIWillSeeThePageTitleAboutOpiniony()
        {
            var check = _aboutPage.GetTitle;
            check.Should().BeTrue();
        }
    }
}
