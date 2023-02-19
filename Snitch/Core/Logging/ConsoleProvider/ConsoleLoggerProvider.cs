using Microsoft.Extensions.Logging;
using System;

namespace Snitch.Core.Logging.ConsoleProvider
{
    public class ConsoleLoggerProvider : ILoggerProvider, IDisposable
    {
        private readonly ConsoleLoggerOptions _options;

        public ConsoleLoggerProvider(ConsoleLoggerOptions options) => this._options = options;

        public void Dispose()
        {
        }

        public ILogger CreateLogger(string categoryName) => (ILogger)new ConsoleLogger(categoryName, this._options);
    }
}
