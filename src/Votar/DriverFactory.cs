using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace Votar
{
    public abstract class DriverFactory
    {
        public RemoteWebDriver GetDriver()
        {
            var service = GetDriverService();
            var options = GetOptions();
            return GetSpecificDriver(service, options);
        }

        protected abstract RemoteWebDriver GetSpecificDriver(DriverService service, DriverOptions options);
        protected abstract DriverService GetDriverService();
        protected abstract DriverOptions GetOptions();
    }
}