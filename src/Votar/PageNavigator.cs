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
        private readonly IDriverFactoryChooser driverFactoryChooser;
        private readonly ILogger logger;

        private int errorCount;

        public int VoteCount { get; private set; }

        public PageNavigator(Settings settings, IDriverFactoryChooser driverFactoryChooser, ILogger logger)
        {
            this.driverFactoryChooser = driverFactoryChooser ?? throw new ArgumentNullException(nameof(driverFactoryChooser));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
                logger.LogError(ex);
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
            var driverFactory = driverFactoryChooser.GetDriverFactory(VoteCount, errorCount);
            var driver = driverFactory.GetDriver();
            return driver;
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
    }
}