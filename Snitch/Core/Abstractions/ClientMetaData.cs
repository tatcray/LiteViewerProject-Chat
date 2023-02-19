using System;
using System.Net.Sockets;

namespace Snitch.Core.Abstractions
{
    [Serializable]
    public class ClientMetaData
    {
        public ClientMetaData(Socket clientSocket, string sessionId)
        {
            this.ClientSocket = clientSocket;
            this.SessionId = sessionId;
            this.WhantToConnect = false;
        }

        public Socket ClientSocket { get; set; }

        public DateTime LastPingTime { get; set; }

        public bool ClientOnline { get; set; }

        public bool WhantToConnect { get; set; }
        public bool GetConnectString { get; set; }

        public bool ConnectPermission { get; set; }
        public string Reason { get; set; }
        public string Error { get; set; }

        public string SessionId { get; }
        public string ID { get; set; }
        public string InitiatorID { get; set; }
        public string ConnectPass { get; set; }
        public string ConnectString { get; set; }
        public string WLConnectString { get; set; }

        public string ClipboardServerPort { get; set; }

        public bool ShareBufferBool { get; set; }
        public bool ShareTextBufferBool { get; set; }
        public bool ShareAudioBool { get; set; }
        public bool WhantAuth { get; set; }
        public string AuthLogin { get; set; }
        public string AuthPass { get; set; }
        public string Json { get; set; }
        
    }
}
