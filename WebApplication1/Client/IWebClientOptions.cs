using System;

namespace Snitch.Core.Abstractions.Client
{
    public interface IWebClientOptions
    {
        string ServerIpv4 { get; set; }

        int ServerPort { get; set; }

        int BufferSize { get; set; }

        TimeSpan StandartReceiveTimeout { get; set; }
        TimeSpan MaxResponseTimeout { get; set; } 
    }
}
