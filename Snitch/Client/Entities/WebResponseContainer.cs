using Snitch.Core.Abstractions;
using System;

namespace Snitch.Client.Entities
{
    [Serializable]
    public class WebResponseContainer<TResponseData> where TResponseData : IResponseData
    {
        public WebResponseContainer(WebResponse<TResponseData> response, DateTime responseReceivedTime)
        {
            this.Response = response;
            this.ResponseReceivedTime = responseReceivedTime;
        }

        public WebResponse<TResponseData> Response { get; }

        public DateTime ResponseReceivedTime { get; }
    }
}
