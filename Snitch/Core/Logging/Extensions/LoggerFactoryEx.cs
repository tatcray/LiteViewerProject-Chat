using Microsoft.Extensions.Logging;
using Snitch.Core.Logging.ConsoleProvider;
using Snitch.Core.Logging.FileProvider;

namespace Snitch.Core.Logging.Extensions
{
    public static class LoggerFactoryEx
    {
        public static void AddConsoleProvider(
          this ILoggerFactory loggerFactory,
          ConsoleLoggerOptions options)
        {
            ConsoleLoggerProvider consoleLoggerProvider = new ConsoleLoggerProvider(options);
            loggerFactory.AddProvider((ILoggerProvider)consoleLoggerProvider);
        }

        public static void AddFileProvider(this ILoggerFactory loggerFactory, FileLoggerOptions options)
        {
            FileLoggerProvider fileLoggerProvider = new FileLoggerProvider(options);
            loggerFactory.AddProvider((ILoggerProvider)fileLoggerProvider);
        }
    }
}
