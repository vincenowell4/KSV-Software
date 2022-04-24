using OpenQA.Selenium;
using SpecFlow.Actions.Selenium;

namespace BDDTests.PageObjects
{
    public class AccessPage : Page
    {

        private IWebElement VoteResultsInputBox => _browserInteractions.WaitAndReturnElement(By.Id("results-input"));
        private IWebElement SubmitVoteInputBox => _browserInteractions.WaitAndReturnElement(By.Id("submit-input"));

        private IWebElement ResultsSubmitButton => _browserInteractions.WaitAndReturnElement(By.Id("results-submit"));

        public AccessPage(IBrowserInteractions browserInteractions)
            : base(browserInteractions)
        {
            PageName = Common.AccessPageName;
        }

        public Boolean GetVoteResultsInputBox => VoteResultsInputBox.Displayed;
        public Boolean GetSubmitVoteInputBox => SubmitVoteInputBox.Displayed;

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
