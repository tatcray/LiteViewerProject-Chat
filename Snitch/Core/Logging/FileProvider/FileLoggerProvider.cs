using Microsoft.Extensions.Logging;
using Snitch.Core.Logging.Events.lers;
using System;

namespace Snitch.Core.Logging.FileProvider
{
    public class FileLoggerProvider : ILoggerProvider, IDisposable
    {
        private readonly FileLoggerOptions _options;

        public FileLoggerProvider(FileLoggerOptions options) => this._options = options;

        public event LogMessageEventler OnLogMessage;

        public void Dispose()
        {
        }

        public ILogger CreateLogger(string categoryName)
        {
            FileLogger fileLogger = new FileLogger(categoryName, this._options);
            fileLogger.OnMessage += this.OnLogMessage;
            return (ILogger)fileLogger;
        }
    }
}
