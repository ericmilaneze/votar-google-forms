using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

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
            return options;
        }
    }
}