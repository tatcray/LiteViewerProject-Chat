using System;

namespace Snitch.Client.Service
{
    [Serializable]
    public class ServiceOptions
    {
        public TimeSpan PingDelay { get; set; }
        public TimeSpan PingTimeout { get; set; }
    }
}
