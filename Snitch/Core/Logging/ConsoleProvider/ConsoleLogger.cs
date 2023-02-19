using Microsoft.Extensions.Logging;
using Snitch.Core.Logging.Abstractions;
using Snitch.Core.Logging.Events.Args;
using Snitch.Core.Logging.Events.lers;
using Snitch.Core.Logging.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Snitch.Core.Logging.ConsoleProvider
{
    internal class ConsoleLogger : ILogger, INotifyLogger
    {
        private static readonly object ConsoleLocker = new object();
        private readonly string _categoryName;
        private readonly ConsoleLoggerOptions _options;

        public ConsoleLogger(string categoryName, ConsoleLoggerOptions options)
        {
            this._categoryName = categoryName;
            this._options = options;
        }

        public event LogMessageEventler OnMessage;

        public void Log<TState>(
          LogLevel logLevel,
          EventId eventId,
          TState state,
          Exception exception,
          Func<TState, Exception, string> formatter)
        {
            LogMessageModel logMessageModel = LoggerHelper.ToLogMessageModel(string.Format("{0}", (object)state), logLevel, this._categoryName, ConsoleScope.Scopes, DateTime.Now);
            LogMessageEventler onMessage = this.OnMessage;
            if (onMessage != null)
                onMessage((object)this, new LogMessageEventArgs()
                {
                    LogLevel = logLevel,
                    Message = string.Format("{0}", (object)state)
                });
            this.LogToConsole(logMessageModel);
        }

        private void LogToConsole(LogMessageModel model)
        {
            lock (ConsoleLogger.ConsoleLocker)
            {
                ConsoleColor foregroundColor = Console.ForegroundColor;
                if (this._options.IsLogTime)
                {
                    string str = string.Format("[{0:dd-MM HH:mm:ss}]", (object)model.LogTime);
                    Console.ForegroundColor = this._options.TimeColor;
                    Console.Write(str);
                }
                string str1 = "[" + model.ClassNameOnly + "]";
                Console.ForegroundColor = this._options.ClassColor;
                Console.Write(str1);
                if (this._options.IsLogMethods)
                {
                    string str2 = "[" + this.ToMethodsLogMessage((IEnumerable<string>)model.MethodsSequence) + "]";
                    Console.ForegroundColor = this._options.MethodColor;
                    Console.Write(str2);
                }
                if (this._options.IsLogLogLevel)
                {
                    string str2 = string.Format("[{0}]", (object)model.LogLevel);
                    Console.ForegroundColor = this._options.LogLevelColor;
                    Console.Write(str2);
                }
                Console.Write(": ");
                string str3 = model.LogMessage ?? "";
                Console.ForegroundColor = this._options.LogLevel2MessageColor[model.LogLevel];
                Console.WriteLine(str3);
                Console.ForegroundColor = foregroundColor;
            }
        }

        private string ToMethodsLogMessage(IEnumerable<string> methods) => methods.Aggregate<string, string>("", (Func<string, string, string>)((current, method) => current + "[" + method + "]"));

        public bool IsEnabled(LogLevel logLevel) => true;

        public IDisposable BeginScope<TState>(TState state) => (IDisposable)new ConsoleScope(string.Format("{0}", (object)state));
    }
}
