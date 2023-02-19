using System;

namespace Snitch.Server.Events.Args
{
    [Serializable]
    public class ClientDisconnectedEventArgs
    {
        public string SessionId { get; set; }
    }
}
