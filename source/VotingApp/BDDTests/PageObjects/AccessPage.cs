using OpenQA.Selenium;
using SpecFlow.Actions.Selenium;

namespace BDDTests.PageObjects
{
    public class AccessPage : Page
    {

        private IWebElement VoteResultsInputBox => _browserInteractions.WaitAndReturnElement(By.Id("results-input"));
        private IWebElement SubmitVoteInputBox => _browserInteractions.WaitAndReturnElement(By.Id("submit-input"));
        private IWebElement EnterAccessBox => _browserInteractions.WaitAndReturnElement(By.Id("submit-input"));
        private IWebElement SubmitButton => _browserInteractions.WaitAndReturnElement(By.Id("vote-submit"));
        private IWebElement ResultsSubmitButton => _browserInteractions.WaitAndReturnElement(By.Id("results-submit"));

        public AccessPage(IBrowserInteractions browserInteractions)
            : base(browserInteractions)
        {
            PageName = Common.AccessPageName;
        }

        public Boolean GetVoteResultsInputBox => VoteResultsInputBox.Displayed;
        public Boolean GetSubmitVoteInputBox => SubmitVoteInputBox.Displayed;
        public Boolean HitSubmitButton => SubmitButton.Displayed;
        public Boolean GetAccessVoteInputBox => EnterAccessBox.Displayed;

        public void EnterVoteAccessCode(string code)
        {
            EnterAccessBox.SendKeysWithClear(code);
            HitSubmitButtonOnPage();
        }

        public void HitSubmitButtonOnPage()
        {
            SubmitButton.Click();
        }
        public void EnterVoteResultsAccessCode(string code)
        {
            VoteResultsInputBox.SendKeysWithClear(code);
        }

        public void ClickResultsSubmit()
        {
            ResultsSubmitButton.Click();
        }

    }
}
