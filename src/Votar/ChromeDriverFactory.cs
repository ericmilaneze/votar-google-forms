using OpenQA.Selenium;
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

        protected override DriverService GetDriverService()
        {
            var service = ChromeDriverService.CreateDefaultService(driversDirectory);
            service.HideCommandPromptWindow = true;
            service.SuppressInitialDiagnosticInformation = true;
            return service;
        }

        protected override DriverOptions GetOptions()
        {
            var options = new ChromeOptions();
            options.AddArgument("--silent");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--log-level=3");
            options.AddArgument("--headless");
            return options;
        }

        protected override RemoteWebDriver GetSpecificDriver(DriverService service, DriverOptions options) =>
            new ChromeDriver(service as ChromeDriverService, options as ChromeOptions);
    }
}