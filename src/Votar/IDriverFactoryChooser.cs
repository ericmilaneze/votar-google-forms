namespace Votar
{
    public interface IDriverFactoryChooser
    {
        DriverFactory GetDriverFactory(int voteCount, int errorCount);
    }
}