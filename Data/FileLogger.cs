using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Data
{
    public interface ILogger
    {
        void Log(string message);
    }

    public class FileLogger : ILogger
    {
        private readonly Logger _logger;

        public FileLogger()
        {
            string baseDirectory = Directory.GetCurrentDirectory();
            string logFilePath = Path.Combine(baseDirectory, "log.txt");

            _logger = new LoggerConfiguration()
                .WriteTo.File(
                    logFilePath,
                    buffered: true,
                    flushToDiskInterval: TimeSpan.FromSeconds(5)) 
                .CreateLogger();

            AppDomain.CurrentDomain.ProcessExit += (sender, e) => _logger.Dispose();
            AppDomain.CurrentDomain.DomainUnload += (sender, e) => _logger.Dispose();
        }

        public void Log(string message)
        {
            _logger.Information(message);
        }
    }
}
