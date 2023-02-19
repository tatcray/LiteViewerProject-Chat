using Snitch.Client;
using Snitch.Core.Abstractions;
using System;

namespace Snitch.Core.Web.Request
{
    [Serializable]
    public class SendClientInfoRequest : IRequestData
    {
        public ClientInfo ClientInfo { get; set; }

        public bool ClientOnline { get; set; }
        public bool ServiceOnline { get; set; }

    }
}
