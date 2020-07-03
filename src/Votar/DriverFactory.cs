using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace Votar
{
    public abstract class DriverFactory
    {
        public RemoteWebDriver GetDriver()
        {
            var service = GetDriverService();
            var options = GetOptions();
            return new ChromeDriver(service as ChromeDriverService, options as ChromeOptions);
        }

        protected abstract DriverService GetDriverService();
        protected abstract DriverOptions GetOptions();
    }
}