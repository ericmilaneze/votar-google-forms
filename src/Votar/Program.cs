using System;
using System.IO;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace Votar
{
    class Program
    {
        // downloaded at https://github.com/mozilla/geckodriver/releases/download/v0.26.0/geckodriver-v0.26.0-linux64.tar.gz
        private static string URL = @"https://docs.google.com/forms/d/e/1FAIpQLSfKRTjSrwWi2hC98tDz_tQ5D0ug0bnQOiSOo9zRjk37zOyj6w/viewform";
        private static string LOGS_PATH = "/home/eric/logs/";
        private static string DRIVERS_DIRECTORY = "/home/eric/bin/";

        private static int voteCount = 0;
        private static int errorCount = 0;

        static void Main(string[] args)
        {
            while (true)
            {
                NavigateToFormAndVote();
                ShowCount();
                DelayBeforeNextIteration();
            }
        }

        private static void NavigateToFormAndVote()
        {
            try
            {
                Vote();
                voteCount++;
            }
            catch (Exception ex)
            {
                LogError(ex);
                errorCount++;
            }
        }

        private static void Vote()
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

        private static RemoteWebDriver GetDriver()
        {
            bool isEven = (voteCount + errorCount) % 2 == 0;
            if (isEven)
                return GetFirefoxDriver();
            return GetChromeDriver();
        }

        private static RemoteWebDriver GetFirefoxDriver()
        {
            var service = GetFirefoxDriverService();
            var options = GetFirefoxOptions();
            return new FirefoxDriver(service, options);
        }

        private static FirefoxDriverService GetFirefoxDriverService()
        {
            var service = FirefoxDriverService.CreateDefaultService(DRIVERS_DIRECTORY);
            service.HideCommandPromptWindow = true;
            service.SuppressInitialDiagnosticInformation = true;
            return service;
        }

        private static FirefoxOptions GetFirefoxOptions()
        {
            var options = new FirefoxOptions();
            options.LogLevel = FirefoxDriverLogLevel.Error;
            return options;
        }

        private static RemoteWebDriver GetChromeDriver()
        {
            var service = GetChromeDriverService();
            var options = GetChromeOptions();
            return new ChromeDriver(service, options);
        }

        private static ChromeDriverService GetChromeDriverService()
        {
            var service = ChromeDriverService.CreateDefaultService(DRIVERS_DIRECTORY);
            service.HideCommandPromptWindow = true;
            service.SuppressInitialDiagnosticInformation = true;
            return service;
        }

        private static ChromeOptions GetChromeOptions()
        {
            var options = new ChromeOptions();
            options.AddArgument("--silent");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--log-level=3");
            return options;
        }

        private static void NavigateToForm(RemoteWebDriver driver) =>
            driver.Navigate().GoToUrl(URL);

        private static void SelectRadioButton(RemoteWebDriver driver)
        {
            Task.Delay(1000).Wait();
            var radioButton = driver.FindElementByCssSelector("div.appsMaterialWizToggleRadiogroupEl.exportToggleEl[data-value~=Suelen]");
            radioButton.Click();
        }

        private static void ClickSubmitButton(RemoteWebDriver driver)
        {
            Task.Delay(1000).Wait();
            var submitButton = driver.FindElementByCssSelector("div.freebirdFormviewerViewNavigationSubmitButton[role=button]");
            submitButton.Click();
        }

        private static void WaitForResponseToLoad(RemoteWebDriver driver)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(4));
            wait.Until(d => d.FindElement(By.CssSelector(".freebirdFormviewerViewResponseLinksContainer a")));
        }

        private static void DelayBeforeClosingForm() =>
            Task.Delay(1000).Wait();

        private static void ShowCount()
        {
            Console.Clear();
            Console.WriteLine($"Contagem: {voteCount}");
        }

        private static void DelayBeforeNextIteration() =>
            Task.Delay(4000).Wait();

        private static void LogError(Exception ex)
        {
            try
            {
                string logFilePath = Path.Combine(LOGS_PATH, $"votar-{DateTime.Now:yyyyMMddHHmmssfff}");
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                    writer.WriteLine(ex.Message);
            }
            catch { }
        }
    }
}
