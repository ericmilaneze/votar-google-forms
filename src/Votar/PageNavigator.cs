using System;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace Votar
{
    public class PageNavigator
    {
        private readonly Settings settings;
        private int errorCount;

        public int VoteCount { get; private set; }

        public PageNavigator(Settings settings)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
            VoteCount = 0;
            errorCount = 0;
        }

        public void NavigateToFormAndVote(Action<Exception> LogError = null)
        {
            try
            {
                Vote();
                VoteCount++;
            }
            catch (Exception ex)
            {
                this.LogError(LogError, ex);
                errorCount++;
            }
        }

        private void Vote()
        {
            using (var driver = GetDriver())
            {
                NavigateToForm(driver);
                SelectRadioButton(driver);
                ClickSubmitButton(driver);
                WaitForResponseToLoad(driver);
                DelayBeforeClosingForm();
            }
        }

        private RemoteWebDriver GetDriver()
        {
            var driverFactory = GetDriverFactory();
            return driverFactory.GetDriver();
        }

        private DriverFactory GetDriverFactory()
        {
            bool isEven = (VoteCount + errorCount) % 2 == 0;
            if (isEven)
                return new FirefoxDriverFactory(settings.DriversDirectory);
            return new ChromeDriverFactory(settings.DriversDirectory);
        }

        private void NavigateToForm(RemoteWebDriver driver) =>
            driver.Navigate().GoToUrl(settings.Url);

        private void SelectRadioButton(RemoteWebDriver driver)
        {
            Task.Delay(1000).Wait();
            var radioButton = driver.FindElementByCssSelector("div.appsMaterialWizToggleRadiogroupEl.exportToggleEl[data-value~=Suelen]");
            radioButton.Click();
        }

        private void ClickSubmitButton(RemoteWebDriver driver)
        {
            Task.Delay(1000).Wait();
            var submitButton = driver.FindElementByCssSelector("div.freebirdFormviewerViewNavigationSubmitButton[role=button]");
            submitButton.Click();
        }

        private void WaitForResponseToLoad(RemoteWebDriver driver)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(4));
            wait.Until(d => d.FindElement(By.CssSelector(".freebirdFormviewerViewResponseLinksContainer a")));
        }

        private void DelayBeforeClosingForm() =>
            Task.Delay(1000).Wait();

        private void LogError(Action<Exception> LogError, Exception ex)
        {
            if (LogError != null)
                LogError.Invoke(ex);
        }
    }
}