using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Snitch.Core.Abstractions.Server
{
    public interface IWebServerAsync : IDisposable
    {
    //    IEnumerable<ClientMetaData> ConnectedClients { get; }

        bool IsStarted { get; }

        int Port { get; }

        void Start();

        WebResponse ManageRequest(
      Snitch.Core.Abstractions.WebRequest webRequest,
      Socket clientSocket);
    }
}
