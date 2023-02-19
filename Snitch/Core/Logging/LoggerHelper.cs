using Microsoft.Extensions.Logging;
using Snitch.Core.Logging.Models;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Snitch.Core.Logging
{
    public static class LoggerHelper
    {
        internal static LogMessageModel ToLogMessageModel(
          string logMessage,
          LogLevel logLevel,
          string fullClassName,
          List<string> methodsSequence,
          DateTime logTime)
        {
            return new LogMessageModel()
            {
                ClassFullName = fullClassName,
                ClassNameOnly = LoggerHelper.GetClassNameOnly(fullClassName),
                LogLevel = logLevel,
                LogTime = logTime,
                MethodsSequence = methodsSequence,
                LogMessage = logMessage
            };
        }

        internal static string GetClassNameOnly(string fullClassName)
        {
            string str = "";
            foreach (char ch in fullClassName)
            {
                str += ch.ToString();
                if (ch == '.')
                    str = "";
            }
            return str;
        }

        public static string GetCaller([CallerMemberName] string caller = null) => caller;
    }
}
