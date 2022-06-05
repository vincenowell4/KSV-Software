using OpenQA.Selenium;
using SpecFlow.Actions.Selenium;

namespace BDDTests.PageObjects
{
    public class AdminPage : Page
    {
        private IWebElement LoggingButton => _browserInteractions.WaitAndReturnElement(By.Id("LogCollapsible"));
        private IWebElement LoggingTable => _browserInteractions.WaitAndReturnElement(By.Id("logTable"));
        public AdminPage(IBrowserInteractions browserInteractions) : base(browserInteractions)
        {
            PageName = Common.AdminPageName;
        }

        public Boolean GetLoggingButton => LoggingButton.Displayed;
        public Boolean GetLoggingTable => LoggingTable.Displayed;

        public void HitLogButton()
        {
            LoggingButton.Click();
        }
    }
}
