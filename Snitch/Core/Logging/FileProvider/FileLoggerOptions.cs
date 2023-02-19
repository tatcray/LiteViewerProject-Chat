using System;

namespace Snitch.Core.Logging.FileProvider
{
    [Serializable]
    public class FileLoggerOptions
    {
        public bool IsLogLogLevel { get; set; } = true;

        public bool IsLogTime { get; set; } = true;

        public bool IsLogMethods { get; set; } = false;

        public string LogFilePath { get; set; } = "Logs.txt";
    }
}
