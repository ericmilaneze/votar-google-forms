using OpenQA.Selenium.Remote;

namespace Votar
{
    public interface DriverFactory
    {
        RemoteWebDriver GetDriver();
    }
}