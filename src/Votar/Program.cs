using System;
using System.Threading.Tasks;

namespace Votar
{
    class Program
    {
        // downloaded at https://github.com/mozilla/geckodriver/releases/download/v0.26.0/geckodriver-v0.26.0-linux64.tar.gz
        private static string URL = @"https://docs.google.com/forms/d/e/1FAIpQLSfKRTjSrwWi2hC98tDz_tQ5D0ug0bnQOiSOo9zRjk37zOyj6w/viewform";
        private static string LOGS_PATH = "/home/eric/logs/";
        private static string DRIVERS_DIRECTORY = "/home/eric/bin/";

        static void Main(string[] args)
        {
            var settings = GetSettings();
            var driverFactoryChooser = new DriverFactoryChooser(settings.DriversDirectory);
            var logger = new Logger(settings.LogsPath);

            var pagaeNavigator = new PageNavigator(settings, driverFactoryChooser, logger);

            while (true)
            {
                try
                {
                    Vote(pagaeNavigator);
                }
                catch
                {
                    DelayBeforeNextIteration();
                }
            }
        }

        private static Settings GetSettings() =>
            new Settings
            {
                Url = URL,
                LogsPath = LOGS_PATH,
                DriversDirectory = DRIVERS_DIRECTORY
            };

        private static void Vote(PageNavigator pagaeNavigator)
        {
            pagaeNavigator.NavigateToFormAndVote();
            ShowCount(pagaeNavigator.VoteCount);
            DelayBeforeNextIteration();
        }

        private static void ShowCount(int voteCount)
        {
            Console.Clear();
            Console.WriteLine($"Contagem: {voteCount}");
        }

        private static void DelayBeforeNextIteration() =>
            Task.Delay(4000).Wait();
    }
}
