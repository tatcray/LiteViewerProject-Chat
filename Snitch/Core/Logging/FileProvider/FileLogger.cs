using Microsoft.Extensions.Logging;
using Snitch.Core.Logging.Abstractions;
using Snitch.Core.Logging.Events.Args;
using Snitch.Core.Logging.Events.lers;
using Snitch.Core.Logging.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Snitch.Core.Logging.FileProvider
{
    public class FileLogger : ILogger, INotifyLogger
    {
        private static readonly object FileAccessKey = new object();
        private readonly string _categoryName;
        private readonly FileLoggerOptions _options;

        public FileLogger(string categoryName, FileLoggerOptions options)
        {
            this._categoryName = categoryName;
            this._options = options;
        }

        public event LogMessageEventler OnMessage;

        public string FilePath => this._options.LogFilePath;

        public void Log<TState>(
          LogLevel logLevel,
          EventId eventId,
          TState state,
          Exception exception,
          Func<TState, Exception, string> formatter)
        {
            string logMessageString = this.ToLogMessageString(LoggerHelper.ToLogMessageModel(string.Format("{0}", (object)state), logLevel, this._categoryName, FileScope.Scopes, DateTime.Now));
            LogMessageEventler onMessage = this.OnMessage;
            if (onMessage != null)
                onMessage((object)this, new LogMessageEventArgs()
                {
                    LogLevel = logLevel,
                    Message = string.Format("{0}", (object)state)
                });
            lock (FileLogger.FileAccessKey)
                this.WriteLogsToFileSafe(logMessageString);
        }

        private string ToLogMessageString(LogMessageModel model)
        {
            string str1 = "";
            if (this._options.IsLogTime)
                str1 += string.Format("[{0:dd-MM HH:mm:ss}]", (object)model.LogTime);
            string str2 = str1 + "[" + model.ClassNameOnly + "]";
            if (this._options.IsLogMethods)
                str2 = str2 + "[" + this.ToMethodsLogMessage((IEnumerable<string>)model.MethodsSequence) + "]";
            if (this._options.IsLogLogLevel)
                str2 += string.Format("[{0}]", (object)model.LogLevel);
            return str2 + ": " + model.LogMessage;
        }

        private string ToMethodsLogMessage(IEnumerable<string> methods) => methods.Aggregate<string, string>("", (Func<string, string, string>)((current, method) => current + "[" + method + "]"));

        private bool WriteLogsToFileSafe(string message)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(this._options.LogFilePath))
                    File.AppendAllText(this._options.LogFilePath, message + Environment.NewLine);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public IDisposable BeginScope<TState>(TState state) => (IDisposable)new FileScope(string.Format("{0}", (object)state));
    }
}
