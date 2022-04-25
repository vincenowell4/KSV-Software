using OpenQA.Selenium;
using SpecFlow.Actions.Selenium;

namespace BDDTests.PageObjects
{
    public class VoteReviewPage : Page
    {

        private IWebElement VoteResultsInputBox => _browserInteractions.WaitAndReturnElement(By.Id("results-input"));
        private IEnumerable<IWebElement> Tableitems => _browserInteractions.WaitAndReturnElements(By.CssSelector("#accessTable tr"));

        public VoteReviewPage(IBrowserInteractions browserInteractions)
            : base(browserInteractions)
        {
            PageName = Common.AccessPageName;
        }
    }
}