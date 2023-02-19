using System;

namespace Snitch.Core.Abstractions
{
    [Serializable]
    public class WebResponse<TResponseData> where TResponseData : IResponseData
    {
        public TResponseData Data { get; set; }

        public string CallingRequestId { get; set; }

        public Exception Exception { get; set; }

        public bool IsOk => this.Exception == null;

        public static WebResponse<TResponseData> Cast(WebResponse<IResponseData> webResponse) => new WebResponse<TResponseData>()
        {
            Data = (TResponseData)webResponse.Data,
            Exception = webResponse.Exception,
            CallingRequestId = webResponse.CallingRequestId
        };
        public static WebResponse<TResponseData> Cast(WebResponse webResponse, TResponseData data) => new WebResponse<TResponseData>()
        {
            Data = data,
            Exception = webResponse.Exception,
            CallingRequestId = webResponse.CallingRequestId
        };
        public static WebResponse<TResponseData> GetFailed(
          Exception exception,
          string requestId)
        {
            return exception != null ? new WebResponse<TResponseData>()
            {
                Data = default(TResponseData),
                Exception = exception,
                CallingRequestId = requestId
            } : throw new ArgumentNullException("exception can't be null");
        }
    }

    [Serializable]
    public class WebResponse 
    {
        public byte[] Data { get; set; }

        public string Type { get; set; }

        public string CallingRequestId { get; set; }

        public Exception Exception { get; set; }

        public bool IsOk => this.Exception == null;
        public bool NeedAnswer { get; set; } = true;

        public static WebResponse GetFailed(
          Exception exception,
          string requestId)
        {
            return exception != null ? new WebResponse()
            {
                Data = Array.Empty<byte>(),
                Type = "Fail",
                Exception = exception,
                CallingRequestId = requestId
            } : throw new ArgumentNullException("exception can't be null");
        }
    }
}
