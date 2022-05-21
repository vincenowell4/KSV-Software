using OpenQA.Selenium;
using SpecFlow.Actions.Selenium;

namespace BDDTests.PageObjects
{
    public class AboutPage : Page
    {

        private IWebElement PageTitle => _browserInteractions.WaitAndReturnElement(By.Id("page-title"));

        public AboutPage(IBrowserInteractions browserInteractions)
            : base(browserInteractions)
        {
            PageName = Common.AboutPageName;
        }

        public Boolean GetTitle => PageTitle.Displayed;

    }
}
