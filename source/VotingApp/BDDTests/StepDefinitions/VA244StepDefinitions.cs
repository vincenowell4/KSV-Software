using BDDTests.PageObjects;
using System;
using TechTalk.SpecFlow;

namespace BDDTests.StepDefinitions
{
    [Binding]
    public class VA244StepDefinitions
    {
        // The context is shared between all step definition files.
        // This is where we put data that is shared between steps in different files.
        private readonly ScenarioContext _scenarioContext;
        private readonly LoginPage _loginPage;
        private readonly CreatePage _createPage;
        private readonly HomePage _homePage;

        public VA244StepDefinitions(ScenarioContext context, LoginPage loginPage, HomePage homePage, CreatePage createPage)
        {
            _scenarioContext = context;
            _loginPage = loginPage;
            _homePage = homePage;
            _createPage = createPage;
        }
        [Then(@"I will see a select list for timezones")]
        public void ThenIWillSeeASelectListForTimezones()
        {
            var dropdown = _createPage.GetTimeZoneDropDown;
            dropdown.Should().BeTrue();
        }

        [Then(@"Pacific Standard Time will be selected by default")]
        public void ThenPacificStandardTimeWillBeSelectedByDefault()
        {
            var items = _createPage.GetTimeZoneItems(0);
            items.Should().Be("Pacific Standard Time");
        }

    }
}
