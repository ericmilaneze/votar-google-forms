namespace Votar
{
    public class DriverFactoryChooser : IDriverFactoryChooser
    {
        private readonly string driversDirectory;

        public DriverFactoryChooser(string driversDirectory)
        {
            this.driversDirectory = driversDirectory;
        }

        public DriverFactory GetDriverFactory(int voteCount, int errorCount)
        {
            bool isEven = (voteCount + errorCount) % 2 == 0;
            if (isEven)
                return new FirefoxDriverFactory(driversDirectory);
            return new ChromeDriverFactory(driversDirectory);
        }
    }
}