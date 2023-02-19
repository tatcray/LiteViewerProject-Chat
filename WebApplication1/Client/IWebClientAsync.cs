using System;
using System.Threading.Tasks;

namespace Snitch.Core.Abstractions.Client
{
    public interface IWebClientAsync : IDisposable
    {
        void SetPort(int port);
        Task<bool> TryConnectAsync();

        Task<bool> PingAsync();

        Task ManageDisconnectAsync();
        Task ManageConnectionAsync();

        Task Abort();

        Task<bool> GetIsConnected();

        Task<bool> GetNeewWhat();

        Task<WebResponse> SendRequestAsync(
          byte[] requestData, string requestType);
         // where TResponseData : IResponseData;

        Task<WebResponse> SendRequestAsyncControl(
          byte[] requestData, string requestType, TimeSpan timeSpan);
        //where TResponseData : IResponseData;
        Task<WebResponse> SendRequestAsyncControl(
          byte[] requestData, string requestType, int Count);
        //where TResponseData : IResponseData;
        Task<WebResponse> SendRequestAsyncControl(
          byte[] requestData, string requestType, int Count, TimeSpan timeSpan);
          //where TResponseData : IResponseData;
    }
}
