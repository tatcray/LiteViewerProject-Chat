using Microsoft.Extensions.Logging;

namespace Snitch.Core.Logging.Events.Args
{
    public class LogMessageEventArgs
    {
        public LogLevel LogLevel { get; set; }

        public string Message { get; set; }
    }
}
