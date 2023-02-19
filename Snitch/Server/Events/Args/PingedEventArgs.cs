using System;

namespace Snitch.Server.Events.Args
{
    [Serializable]
    public class PingedEventArgs
    {
        public string SessionId { get; set; }
    }
}
