using System;
using System.IO;
using System.Threading;
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

        static void Main(string[] args)
        {
            int voteCount = 0;
            int errorCount = 0;

            while (true)
            {
                try
                {
                    using (var driver = GetDriver(voteCount, errorCount))
                        VoteInPage(driver);
                    voteCount++;
                }
                catch (Exception ex)
                {
                    LogError(ex);
                    errorCount++;
                }

                Console.Clear();
                Console.WriteLine($"Contagem: {voteCount}");
                Task.Delay(4000).Wait();
            }
        }

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

        private static void VoteInPage(RemoteWebDriver driver)
        {
            driver.Navigate().GoToUrl(URL);

            Task.Delay(1000).Wait();
            var radioButton = driver.FindElementByCssSelector("div.appsMaterialWizToggleRadiogroupEl.exportToggleEl[data-value~=Suelen]");
            radioButton.Click();

            Task.Delay(1000).Wait();
            var submitButton = driver.FindElementByCssSelector("div.freebirdFormviewerViewNavigationSubmitButton[role=button]");
            submitButton.Click();

            WaitForResponse(driver);
            Task.Delay(1000).Wait();
        }

        private static void WaitForResponse(RemoteWebDriver driver)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(1));
                wait.Until(d => d.FindElement(By.CssSelector(".freebirdFormviewerViewResponseLinksContainer a")));
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        private static RemoteWebDriver GetDriver(int voteCount, int errorCount)
        {
            bool isEven = (voteCount + errorCount) % 2 == 0;
            if (isEven)
                return GetFirefoxDriver();
            return GetChromeDriver();
        }

        private static RemoteWebDriver GetFirefoxDriver()
        {
            var service = FirefoxDriverService.CreateDefaultService(DRIVERS_DIRECTORY);
            service.HideCommandPromptWindow = true;
            service.SuppressInitialDiagnosticInformation = true;

            var options = new FirefoxOptions();
            options.LogLevel = FirefoxDriverLogLevel.Error;

            return new FirefoxDriver(service, options);
        }

        private static RemoteWebDriver GetChromeDriver()
        {
            var service = ChromeDriverService.CreateDefaultService(DRIVERS_DIRECTORY);
            service.HideCommandPromptWindow = true;
            service.SuppressInitialDiagnosticInformation = true;

            var options = new ChromeOptions();
            options.AddArgument("--silent");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--log-level=3");

            return new ChromeDriver(service);
        }
    }
}
