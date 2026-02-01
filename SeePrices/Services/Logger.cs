using System;
using System.IO;

namespace SeePrices.Services
{
    public static class Logger
    {
        private static string logDirectory = "logs";

        private static string fileName = $"log_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt";

        private static string filePath = Path.Combine(logDirectory, fileName);

        // Ensure log directory exists and initialize log file for this execution
        static Logger()
        {
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
        }

        // Append log entry to file with timestamp and severity level
        public static void Log(string message, string level = "INFO")
        {
            string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}";

            try
            {
                File.AppendAllText(filePath, logEntry + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Log error: {ex.Message}");
            }
        }
    }
}