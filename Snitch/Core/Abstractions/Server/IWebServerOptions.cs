using System;

namespace Snitch.Core.Abstractions.Server
{
    public interface IWebServerOptions
    {
        int BufferSize { get; set; }

        int ServerPort { get; set; }

        TimeSpan PingTimeout { get; set; }

        string DownloadDirectory { get; set; }
    }
}
