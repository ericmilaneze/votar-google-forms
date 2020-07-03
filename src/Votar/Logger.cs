using System;
using System.IO;

namespace Votar
{
    public class Logger : ILogger
    {
        private readonly string logsPath;

        public Logger(string logsPath)
        {
            if (string.IsNullOrWhiteSpace(logsPath))
                throw new ArgumentException($"'{nameof(logsPath)}' cannot be null or whitespace", nameof(logsPath));

            this.logsPath = logsPath;
        }

        public void LogError(Exception ex)
        {
            try
            {
                string logFilePath = Path.Combine(logsPath, $"votar-{DateTime.Now:yyyyMMddHHmmssfff}");
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                    writer.WriteLine(ex.Message);
            }
            catch { }
        }
    }
}