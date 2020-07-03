using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace Votar
{
    public class ChromeDriverFactory : DriverFactory
    {
        private readonly string driversDirectory;

        public ChromeDriverFactory(string driversDirectory)
        {
            this.driversDirectory = driversDirectory;
        }

        public RemoteWebDriver GetDriver()
        {
            var service = GetChromeDriverService();
            var options = GetChromeOptions();
            return new ChromeDriver(service, options);
        }

        private ChromeDriverService GetChromeDriverService()
        {
            var service = ChromeDriverService.CreateDefaultService(driversDirectory);
            service.HideCommandPromptWindow = true;
            service.SuppressInitialDiagnosticInformation = true;
            return service;
        }

        private ChromeOptions GetChromeOptions()
        {
            var options = new ChromeOptions();
            options.AddArgument("--silent");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--log-level=3");
            return options;
        }
    }
}