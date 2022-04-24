using OpenQA.Selenium;
using SpecFlow.Actions.Selenium;

namespace BDDTests.PageObjects
{
    public class ResultsPage : Page
    {

        private IWebElement BackToAccessButton => _browserInteractions.WaitAndReturnElement(By.Id("back-button"));

        public ResultsPage(IBrowserInteractions browserInteractions)
            : base(browserInteractions)
        {
            PageName = Common.ResultsPageName;
        }

        public void ClickBackToAccess()
        {
            //Click the back to access page button
            BackToAccessButton.Click();
        }

    }
}

