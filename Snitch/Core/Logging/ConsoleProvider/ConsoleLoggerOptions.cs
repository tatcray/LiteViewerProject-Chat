using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Snitch.Core.Logging.ConsoleProvider
{
    [Serializable]
    public class ConsoleLoggerOptions
    {
        public bool IsLogTime { get; set; } = true;

        public bool IsLogMethods { get; set; } = false;

        public bool IsLogLogLevel { get; set; } = true;

        public ConsoleColor ClassColor { get; set; } = ConsoleColor.DarkGray;

        public ConsoleColor MethodColor { get; set; } = ConsoleColor.DarkGray;

        public ConsoleColor LogLevelColor { get; set; } = ConsoleColor.DarkGray;

        public ConsoleColor TimeColor { get; set; } = ConsoleColor.DarkGray;

        public Dictionary<LogLevel, ConsoleColor> LogLevel2MessageColor { get; set; } = new Dictionary<LogLevel, ConsoleColor>()
    {
      {
        LogLevel.Critical,
        ConsoleColor.Red
      },
      {
        LogLevel.Error,
        ConsoleColor.Red
      },
      {
        LogLevel.Debug,
        ConsoleColor.DarkCyan
      },
      {
        LogLevel.Trace,
        ConsoleColor.Gray
      },
      {
        LogLevel.Information,
        ConsoleColor.Cyan
      },
      {
        LogLevel.Warning,
        ConsoleColor.Yellow
      },
      {
        LogLevel.None,
        ConsoleColor.DarkGray
      }
    };
    }
}
