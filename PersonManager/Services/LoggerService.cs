using Microsoft.Extensions.Logging;

namespace PersonManager.Services
{
    public class LoggerService : ILoggerService
    {
        private readonly ILogger<LoggerService> _logger;
        private readonly string? _filePath;

        public LoggerService(ILogger<LoggerService> logger, string? filePath = null)
        {
            _logger = logger;
            _filePath = filePath;
        }

        private void WriteToFile(string level, string message)
        {
            if (!string.IsNullOrEmpty(_filePath))
            {
                var logLine = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}\n";
                System.IO.File.AppendAllText(_filePath, logLine);
            }
        }

        public void LogInfo(string message)
        {
            _logger.LogInformation(message);
            WriteToFile("INFO", message);
        }

        public void LogWarning(string message)
        {
            _logger.LogWarning(message);
            WriteToFile("WARN", message);
        }

        public void LogError(string message)
        {
            _logger.LogError(message);
            WriteToFile("ERROR", message);
        }

        public void LogDebug(string message)
        {
            _logger.LogDebug(message);
            WriteToFile("DEBUG", message);
        }
    }
}
