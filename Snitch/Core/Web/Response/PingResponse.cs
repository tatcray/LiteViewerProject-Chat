using Snitch.Core.Abstractions;
using System;

namespace Snitch.Core.Web.Response
{
    [Serializable]
    public class PingResponse : IResponseData
    {
        public bool needWhat { get; set; }

        //public string ApiStatus { get; set; }
        //public bool Ok { get; set; }
    }
}
