using BDDTests.PageObjects;
using System;
using TechTalk.SpecFlow;

namespace BDDTests.StepDefinitions
{
    [Binding]
    public class VA242StepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly LoginPage _loginPage;

        public VA242StepDefinitions(ScenarioContext context, LoginPage loginPage)
        {
            _scenarioContext = context;
            _loginPage = loginPage;
        }

        [Given(@"I click on the Login link")]
        public void GivenIClickOnTheLoginLink()
        {
            _loginPage.Goto();
        }

        [Then(@"I will see the Login page with a reCAPTCHA widget")]
        public void ThenIWillSeeTheLoginPageWithAReCAPTCHAWidget()
        {
            var check = _loginPage.GetRecaptcha;
            check.Should().BeTrue();

        }
    }
}
