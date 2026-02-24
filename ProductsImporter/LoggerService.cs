using System;
using System.IO;
using System.Threading.Tasks;

namespace ProductsImporter
{
    public class LoggerService
    {
        private readonly string _logFile;
        public event Action<string> OnLog;

        public LoggerService(string path = null)
        {
            var dir = path ?? AppDomain.CurrentDomain.BaseDirectory;
            _logFile = Path.Combine(dir, "import.log");
        }

        public void Log(string message)
        {
            var line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
            try
            {
                File.AppendAllText(_logFile, line + Environment.NewLine);
            }
            catch
            {
                // ignore file logging errors
            }

            try
            {
                OnLog?.Invoke(line);
            }
            catch
            {
                // ignore handlers errors
            }
        }

        public Task LogAsync(string message)
        {
            return Task.Run(() => Log(message));
        }
    }
}
