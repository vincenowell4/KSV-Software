using OpenQA.Selenium;
using SpecFlow.Actions.Selenium;

namespace BDDTests.PageObjects
{
    public class CreatePage : Page
    {
        // Select all the elements needed for tests
        private IWebElement VoteStartNow => _browserInteractions.WaitAndReturnElement(By.Id("VoteStartNow"));
        private IWebElement VoteStartFuture => _browserInteractions.WaitAndReturnElement(By.Id("VoteStartFuture"));
        private IWebElement VoteOpenDateTime => _browserInteractions.WaitAndReturnElement(By.Id("VoteOpenDateTime"));

        public Boolean GetVoteStartNowDisplayed => VoteStartNow.Displayed;
        public Boolean GetVoteStartFutureDisplayed => VoteStartFuture.Displayed;
        public Boolean GetVoteOpenDateTimeEnabled => VoteOpenDateTime.Enabled;

        public string GetFutureDateText => VoteOpenDateTime.Text;



        public CreatePage(IBrowserInteractions browserInteractions)
            : base(browserInteractions)
        {
            PageName = Common.CreatePageName;
        }

        public void StartVoteNowClick()
        {
            VoteStartNow.Click();
        }

        public void StartVoteFutureClick()
        {
            VoteStartFuture.Click();
        }

        public void EnterFutureDate(string date)
        {
            VoteOpenDateTime.SendKeysWithClear(date);
        }

    }
}