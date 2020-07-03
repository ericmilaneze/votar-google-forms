using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;

namespace Votar
{
    public class FirefoxDriverFactory : DriverFactory
    {
        private readonly string driversDirectory;

        public FirefoxDriverFactory(string driversDirectory)
        {
            this.driversDirectory = driversDirectory;
        }

        public RemoteWebDriver GetDriver()
        {
            var service = GetFirefoxDriverService();
            var options = GetFirefoxOptions();
            return new FirefoxDriver(service, options);
        }

        private FirefoxDriverService GetFirefoxDriverService()
        {
            var service = FirefoxDriverService.CreateDefaultService(driversDirectory);
            service.HideCommandPromptWindow = true;
            service.SuppressInitialDiagnosticInformation = true;
            return service;
        }

        private FirefoxOptions GetFirefoxOptions()
        {
            var options = new FirefoxOptions();
            options.LogLevel = FirefoxDriverLogLevel.Error;
            return options;
        }
    }
}