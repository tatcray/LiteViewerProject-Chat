using Snitch.Core.Abstractions.Client;
using System;

namespace Snitch.Client.Entities
{
    [Serializable]
    public class DefaultWebClientOptions : IWebClientOptions
    {
        public DefaultWebClientOptions(string serverIpv4, int serverPort)
        {
            this.ServerIpv4 = serverIpv4;
            this.ServerPort = serverPort;
        }

        public string ServerIpv4 { get; set; }

        public int ServerPort { get; set; }

        public int BufferSize { get; set; } = 256000;
        public TimeSpan StandartReceiveTimeout { get; set; } = TimeSpan.FromSeconds(20);
        public TimeSpan MaxResponseTimeout { get; set; } = TimeSpan.FromSeconds(70);
    }
}
