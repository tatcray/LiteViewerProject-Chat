using System;

namespace Snitch.Server.Events.Args
{
    [Serializable]
    public class ClientConnectedEventArgs
    {
        public string SessionId { get; set; }
    }
}
