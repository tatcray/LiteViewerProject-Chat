using Snitch.Core.Logging.Events.lers;

namespace Snitch.Core.Logging.Abstractions
{
    public interface INotifyLogger
    {
        event LogMessageEventler OnMessage;
    }
}
