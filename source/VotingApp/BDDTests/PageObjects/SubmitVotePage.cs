using OpenQA.Selenium;
using SpecFlow.Actions.Selenium;

namespace BDDTests.PageObjects
{
    public class SubmitVotePage : Page
    {

        private IWebElement errorMessage => _browserInteractions.WaitAndReturnElement(By.Id("errorMessage"));
        private IWebElement updateMessage => _browserInteractions.WaitAndReturnElement(By.Id("updateMessage"));
        public SubmitVotePage(IBrowserInteractions browserInteractions)
            : base(browserInteractions)
        {
            PageName = Common.AccessPageName;
        }

        public string GetErrorMessage() => errorMessage.Text;
        public string GetUpdateMessage() => updateMessage.Text;

    }
}