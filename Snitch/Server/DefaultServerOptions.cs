using Snitch.Core.Abstractions.Server;
using System;

namespace Snitch.Server
{
    [Serializable]
    public class DefaultServerOptions : IWebServerOptions
    {
        public DefaultServerOptions(int port) => this.ServerPort = port;

        public int BufferSize { get; set; } = 2000000;

        public int ServerPort { get; set; }

        public TimeSpan PingTimeout { get; set; } = TimeSpan.FromMinutes(2.0);

        public string DownloadDirectory { get; set; }

    }
}
