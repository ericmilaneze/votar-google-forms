using System;
using System.IO;
using System.Threading.Tasks;

namespace Votar
{
    class Program
    {
        // downloaded at https://github.com/mozilla/geckodriver/releases/download/v0.26.0/geckodriver-v0.26.0-linux64.tar.gz
        private static string URL = @"https://docs.google.com/forms/d/e/1FAIpQLSfKRTjSrwWi2hC98tDz_tQ5D0ug0bnQOiSOo9zRjk37zOyj6w/viewform";
        private static string LOGS_PATH = "/home/eric/logs/";
        private static string DRIVERS_DIRECTORY = "/home/eric/bin/";

        private static Settings settings = 
            new Settings
            {
                Url = URL,
                LogsPath = LOGS_PATH,
                DriversDirectory = DRIVERS_DIRECTORY
            };

        static void Main(string[] args)
        {
            var pagaeNavigator = new PageNavigator(settings);

            while (true)
            {
                try
                {
                    Vote(pagaeNavigator);
                }
                catch (Exception) 
                {
                    DelayBeforeNextIteration();
                }
            }
        }

        private static void Vote(PageNavigator pagaeNavigator)
        {
            pagaeNavigator.NavigateToFormAndVote(LogError);
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

        private static void LogError(Exception ex)
        {
            try
            {
                string logFilePath = Path.Combine(settings.LogsPath, $"votar-{DateTime.Now:yyyyMMddHHmmssfff}");
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                    writer.WriteLine(ex.Message);
            }
            catch { }
        }
    }
}
