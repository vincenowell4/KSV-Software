using OpenQA.Selenium;
using SpecFlow.Actions.Selenium;
using VotingApp.Models;

namespace BDDTests.PageObjects
{
    public class CreatePage : Page
    {
        // Select all the elements needed for tests
        private IWebElement VoteStartNow => _browserInteractions.WaitAndReturnElement(By.Id("VoteStartNow"));
        private IWebElement VoteStartFuture => _browserInteractions.WaitAndReturnElement(By.Id("VoteStartFuture"));
        private IWebElement VoteOpenDateTime => _browserInteractions.WaitAndReturnElement(By.Id("VoteOpenDateTime"));
        private IWebElement VoteTypeCollapsible => _browserInteractions.WaitAndReturnElement(By.Id("VoteCollapsible"));
        private IWebElement VoteRequiredFieldsHeader => _browserInteractions.WaitAndReturnElement(By.Id("RequiredHeader"));
        private IWebElement VoteTitleField => _browserInteractions.WaitAndReturnElement(By.Id("VoteTitle"));
        private IWebElement VoteDescriptionField => _browserInteractions.WaitAndReturnElement(By.Id("VoteDescription"));
        private IWebElement VoteTypeDropDown => _browserInteractions.WaitAndReturnElement(By.Id("VoteType"));


        public Boolean GetVoteStartNowDisplayed => VoteStartNow.Displayed;
        public Boolean GetVoteStartFutureDisplayed => VoteStartFuture.Displayed;
        public Boolean GetVoteOpenDateTimeEnabled => VoteOpenDateTime.Enabled;
        public Boolean GetVoteTypeCollapsibleDisplayed => VoteTypeCollapsible.Displayed;
        public Boolean GetVoteRequiredHeader => VoteRequiredFieldsHeader.Displayed;
        public Boolean GetVoteTitleArea => VoteTitleField.Displayed;
        public Boolean GetVoteDescriptionArea => VoteDescriptionField.Displayed;
        public Boolean GetVoteTypeDropDown => VoteTypeDropDown.Displayed;
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
            VoteStartFuture.ClickWithRetry();
        }

        public void SetVoteTypeDropDown()
        {
            VoteTypeDropDown.SelectDropdownOptionByValue("Multiple Choice Multi-Round Vote");
        }

        public void EnterFutureDate(string date)
        {
            VoteOpenDateTime.SendKeysWithClear(date);
        }

    }
}