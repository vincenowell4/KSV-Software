using OpenQA.Selenium;
using SpecFlow.Actions.Selenium;

namespace BDDTests.PageObjects
{
    public class HomePage : Page
    {
        private IWebElement Title => _browserInteractions.WaitAndReturnElement(By.Id("title"));
        private IWebElement WelcomeText => _browserInteractions.WaitAndReturnElement(By.Id("manage"));
        private IEnumerable<IWebElement> AppleButtons => _browserInteractions.WaitAndReturnElements(By.CssSelector("#listOfApples button"));
        private IEnumerable<IWebElement> NavbarItems => _browserInteractions.WaitAndReturnElements(By.CssSelector("#navbar li"));
        private IEnumerable<IWebElement> UserItems => _browserInteractions.WaitAndReturnElements(By.CssSelector("#UserItems li"));
        public HomePage(IBrowserInteractions browserInteractions)
            : base(browserInteractions)
        {
            PageName = Common.HomePageName;
        }

        public string GetTitle => Title.Text;
        public string GetWelcomeText => WelcomeText.Text;

        public string GetAppleButtonText(int index) => AppleButtons.ElementAt(index).Text;
        public string GetNavbarItems(int index) => NavbarItems.ElementAt(index).Text;
        public IEnumerable<string> GetNavbarItemsTexts() => NavbarItems.Select(x => x.Text);
        public string GetUserItems(int index) => UserItems.ElementAt(index).Text;
        public IEnumerable<string> GetUserItemsTexts() => UserItems.Select(x => x.Text);
        public IEnumerable<string> GetAppleButtonTexts() => AppleButtons.Select(x => x.Text);

        public void ClickAppleButton(int index)
        {
            AppleButtons.ElementAt(index).Click();
        }

    }
}