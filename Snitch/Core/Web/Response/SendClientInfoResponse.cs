using Snitch.Core.Abstractions;
using System;
using System.Collections.Generic;

namespace Snitch.Core.Web.Response
{
    [Serializable]
    public class SendClientInfoResponse : IResponseData
    {
        //public List<IClientInfo> ConnectedClients { get; set; } = new List<IClientInfo>();
        public int ConnectedClients  { get; set; }
        //public bool Ok { get; set; }
    }
}
