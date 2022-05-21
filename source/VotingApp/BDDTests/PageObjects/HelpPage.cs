using OpenQA.Selenium;
using SpecFlow.Actions.Selenium;

namespace BDDTests.PageObjects
{
    public class HelpPage : Page
    {
        private IWebElement PollTypeAccordian => _browserInteractions.WaitAndReturnElement(By.Id("accord-three"));
        private IWebElement MultiRoundDescription => _browserInteractions.WaitAndReturnElement(By.Id("multi-round-descrption"));
        private IWebElement MultiRoundCreateButton => _browserInteractions.WaitAndReturnElement(By.Id("multi-round-button"));
        private IWebElement FAQTitle => _browserInteractions.WaitAndReturnElement(By.Id("faq-title"));

        private IWebElement FAQCards => _browserInteractions.WaitAndReturnElement(By.Id("faq-cards"));

        public HelpPage(IBrowserInteractions browserInteractions) : base(browserInteractions)
        {
            PageName = Common.HelpPageName;
        }

        public Boolean GetPollTypeAccordian => PollTypeAccordian.Displayed;
        public Boolean GetMultiRoundDescription => MultiRoundDescription.Displayed;
        public Boolean GetFAQTitle => FAQTitle.Displayed;
        public Boolean GetFAQCards => FAQCards.Displayed;

        public void HitMultiAccordian()
        {
            PollTypeAccordian.ClickWithRetry();
        }

        public void HitCreateButton()
        {
            MultiRoundCreateButton.Click();
        }
    }
}

