using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace Votar
{
    public class FirefoxDriverFactory : DriverFactory
    {
        private readonly string driversDirectory;

        public FirefoxDriverFactory(string driversDirectory)
        {
            this.driversDirectory = driversDirectory;
        }

        protected override DriverService GetDriverService()
        {
            var service = FirefoxDriverService.CreateDefaultService(driversDirectory);
            service.HideCommandPromptWindow = true;
            service.SuppressInitialDiagnosticInformation = true;
            return service;
        }

        protected override DriverOptions GetOptions()
        {
            var options = new FirefoxOptions();
            options.LogLevel = FirefoxDriverLogLevel.Error;
            return options;
        }
    }
}