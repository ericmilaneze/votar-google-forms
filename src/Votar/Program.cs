using System;
using System.IO;
using System.Threading;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;

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

            //while (true)
            //{
                using (var driver = GetDriver(voteCount))
                {
                    try
                    {
                        VoteInPage(driver);
                        voteCount++;
                    }
                    catch (Exception ex)
                    {
                        string logFilePath = Path.Combine(LOGS_PATH, "votar-{DateTime.Now:yyyyMMddHHmmssfff}");
                        using (StreamWriter writer = new StreamWriter(logFilePath))
                            writer.WriteLine(ex.Message);
                    }
                }

                Console.Clear();
                Console.WriteLine($"Contagem: {voteCount}");
                Thread.Sleep(2);
            //}   
        }

        private static void VoteInPage(RemoteWebDriver driver)
        {
            driver.Navigate().GoToUrl(URL);
            var radioButton = driver.FindElementByCssSelector("div.appsMaterialWizToggleRadiogroupEl.exportToggleEl[data-value~=Suelen]");
            radioButton.Click();
            var submitButton = driver.FindElementByCssSelector("div.freebirdFormviewerViewNavigationSubmitButton[role=button]");
            submitButton.Click();
        }

        private static RemoteWebDriver GetDriver(int voteCount)
        {
            if (voteCount % 2 == 0)
            {
                // var options = new ChromeOptions();
                // options.AddArgument("--silent");
                // options.AddArgument("--disable-gpu");
                // options.AddArgument("--log-level=3");

                var service = ChromeDriverService.CreateDefaultService(DRIVERS_DIRECTORY);
                service.HideCommandPromptWindow = true;
                service.SuppressInitialDiagnosticInformation = true;

                //return new ChromeDriver(service, options);
                return new ChromeDriver(service);
            }

            return new FirefoxDriver(DRIVERS_DIRECTORY);
        }
    }
}
