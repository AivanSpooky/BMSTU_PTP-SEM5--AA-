using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace conveyor
{
    public class Logger
    {
        private readonly string logDirectory;
        private readonly string stage1LogPath;
        private readonly string stage2LogPath;
        private readonly string stage3LogPath;
        private readonly string collectorLogPath;
        private readonly object stage1Lock = new object();
        private readonly object stage2Lock = new object();
        private readonly object stage3Lock = new object();
        private readonly object collectorLock = new object();

        public Logger(string outputDirectory, string logType)
        {
            logDirectory = outputDirectory;
            Directory.CreateDirectory(logDirectory);
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            stage1LogPath = Path.Combine(logDirectory, $"stage1_{logType}_log_{timestamp}.txt");
            stage2LogPath = Path.Combine(logDirectory, $"stage2_{logType}_log_{timestamp}.txt");
            stage3LogPath = Path.Combine(logDirectory, $"stage3_{logType}_log_{timestamp}.txt");
            collectorLogPath = Path.Combine(logDirectory, $"{logType}_log_{timestamp}.txt");
        }
        public Logger(string outputDirectory)
        {
            logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, outputDirectory);
            Directory.CreateDirectory(logDirectory);
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            stage1LogPath = Path.Combine(logDirectory, $"stage1_log_{timestamp}.txt");
            stage2LogPath = Path.Combine(logDirectory, $"stage2_log_{timestamp}.txt");
            stage3LogPath = Path.Combine(logDirectory, $"stage3_log_{timestamp}.txt");
            collectorLogPath = Path.Combine(logDirectory, $"collector_log_{timestamp}.txt");
        }

        public void LogStage1(string message)
        {
            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} - {message}";
            lock (stage1Lock)
            {
                File.AppendAllText(stage1LogPath, logEntry + Environment.NewLine);
            }
        }

        public void LogStage2(string message)
        {
            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} - {message}";
            lock (stage2Lock)
            {
                File.AppendAllText(stage2LogPath, logEntry + Environment.NewLine);
            }
        }

        public void LogStage3(string message)
        {
            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} - {message}";
            lock (stage3Lock)
            {
                File.AppendAllText(stage3LogPath, logEntry + Environment.NewLine);
            }
        }

        public void LogCollector(string message)
        {
            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} - {message}";
            lock (collectorLock)
            {
                File.AppendAllText(collectorLogPath, logEntry + Environment.NewLine);
            }
        }
    }
}
