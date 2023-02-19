using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Snitch.Core.Logging.Models
{
    [Serializable]
    internal class LogMessageModel
    {
        public LogLevel LogLevel { get; set; }

        public string ClassNameOnly { get; set; }

        public string ClassFullName { get; set; }

        public DateTime LogTime { get; set; }

        public List<string> MethodsSequence { get; set; } = new List<string>();

        public string LogMessage { get; set; }
    }
}
