using System.Net;
using System.Net.Sockets;
using BinaryPack;
using Snitch.Client.Events.Args;
using Snitch.Core;
using Snitch.Core.Abstractions.Client;
using Snitch.Core.Web.Request;
using Snitch.Core.Web.Response;
using WebResponse = Snitch.Core.Abstractions.WebResponse;

namespace Snitch.Client
{
    public class WebClient : IWebClientAsync, IDisposable
  {
    private Socket _serverSocket;

    /*---------------------------------------------------------------------------------------------------------------------*/

    public WebClient(IWebClientOptions options)
    {
      this.Logger = (ILogger) new LoggerFactory().CreateLogger<WebClient>();
      this.Options = options;
      this.SessionId = Guid.NewGuid().ToString();
    }

    public WebClient(IWebClientOptions options, ILogger Logger)
    {
      this.Logger = Logger;
      this.Options = options;
      this.SessionId = Guid.NewGuid().ToString();
    }

        /*---------------------------------------------------------------------------------------------------------------------*/

    
    public string SessionId { get; }

    public DateTime LastPingTime { get; private set; }

    public IWebClientOptions Options { get; set; }

    private ILogger Logger { get; }

        /*---------------------------------------------------------------------------------------------------------------------*/

    public bool InWork { get; private set; } = false;

    public bool IsConnected { get; private set; } = false;

    public bool needWhat { get; set; }

        /*---------------------------------------------------------------------------------------------------------------------*/

//    public event WorkSpaceEventler OnWorkSpaceAsync;

//    public event ConnectedEventler OnConnectedAsync;

//    public event DisconnectedEventler OnDisconnectedAsync;
        /*---------------------------------------------------------------------------------------------------------------------*/
        public async Task<bool> GetIsConnected()
        {
            return IsConnected;
        }

        public async Task<bool> GetNeewWhat()
        {
            return needWhat;
        }

        /*---------------------------------------------------------------------------------------------------------------------*/
        public async Task InitializeAsync()
        {
            await Task.Delay(1);
        }

        public async Task WorkingAsync()
        {
            this.InWork = true;
            while (this.InWork)
            {
                await Task.Delay(5000);
                bool pinged = await this.PingAsync();
                if (!pinged)
                {
                    this.Logger.LogWarning("Ping failed");
                    this.Logger.LogDebug("Connection...");
                    bool connected = await this.TryConnectAsync();
                    if (!connected)
                    {
                        this.Logger.LogWarning("Connection failed");
                        await this.ManageDisconnectAsync();
                        continue;
                    }
                }
                await this.ManageConnectionAsync();
                /*
                if(this.needWhat)
                {
                    await this.NeedWhatAsync();
                }
                */
                if (this.OnWorkSpaceAsync != null)
                    await this.OnWorkSpaceAsync((object)this, new WorkSpaceEventArgs());
            }
        }


        /*---------------------------------------------------------------------------------------------------------------------*/

        public async Task Abort()
        {
            this.InWork = false;
            this.Logger.LogInformation("Web client aborted");
        }

        public async Task ManageDisconnectAsync()
        {
            if (!this.IsConnected)
                return;
            this.IsConnected = false;
            if (this.OnDisconnectedAsync != null)
                await this.OnDisconnectedAsync((object)this, new DisconnectedEventArgs());
        }

        public async Task ManageConnectionAsync()
        {
            if (this.IsConnected)
                return;
            this.IsConnected = true;
            if (this.OnConnectedAsync != null)
                await this.OnConnectedAsync((object)this, new ConnectedEventArgs());
        }

        public async Task<bool> TryConnectAsync()
        {
            using (this.Logger.BeginScope<string>(LoggerHelper.GetCaller(nameof(TryConnectAsync))))
            {
                try
                {
                    await Task.Run(() => this.ConnectAsync());
                }
                catch (Exception ex)
                {
                    this.Logger.LogWarning(ex.Message);
                    return false;
                }
                //bool flag = await this.PingAsync();
                return await this.PingAsync();
            }
        }

        private async Task ConnectAsync()
        {
            this._serverSocket?.Dispose();
            this.Logger.LogDebug("Connection...");
            this._serverSocket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp)
            {
                ReceiveTimeout = (int)this.Options.MaxResponseTimeout.TotalMilliseconds,
            };
            this._serverSocket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);
            this._serverSocket.Connect(IPAddress.Parse(this.Options.ServerIpv4), this.Options.ServerPort);
            this.Logger.LogTrace("Connection completed");
        }

        PingRequest pingRequestData;
        WebResponse pingWebResponse;
        PingResponse pingResponse;
        public async Task<bool> PingAsync()
        {
            using (this.Logger.BeginScope<string>(LoggerHelper.GetCaller(nameof(PingAsync))))
            {
                try
                {
                    pingRequestData = new PingRequest();
                    //pingWebResponse = new WebResponse();
                    //pingResponse = new PingResponse();

                    pingWebResponse = await this.SendRequestAsyncControl(
                        BinaryConverter.Serialize(pingRequestData),
                        pingRequestData.GetType().Name,
                        TimeSpan.FromSeconds(5));

                    if (pingWebResponse.Exception != null)
                        throw pingWebResponse.Exception;

                    pingResponse = BinaryConverter.Deserialize<PingResponse>(pingWebResponse.Data);

                    if (!string.IsNullOrWhiteSpace(pingResponse.ErrorString))
                        throw new Exception("|OS|" + pingResponse.ErrorString);

                    this.Logger.LogDebug("Pinged");
                    this.LastPingTime = DateTime.Now;
                    this.needWhat = pingResponse.needWhat;
                    pingRequestData = null;
                    pingWebResponse = null;
                    pingResponse = null;
                    return true;
                }
                catch(Exception err)
                {
                    this.Logger.LogWarning("Ping failed: " + err.Message);
                    pingRequestData = null;
                    pingWebResponse = null;
                    pingResponse = null;
                    return false;
                }
                
                
            }
        }



        /*---------------------------------------------------------------------------------------------------------------------*/
       
        public async Task<WebResponse> SendRequestAsync(
      byte[] requestData, string requestType)
      //where TResponseData : IResponseData
        {
            Core.Abstractions.WebRequest request = this.GenerateRequest(requestData, requestType);
            WebResponse webResponse = await this.SendRequestInternalAsync(request);
            request = (Core.Abstractions.WebRequest)null;
            return webResponse;
        }

        public async Task<WebResponse> SendRequestInternalAsync(
      Core.Abstractions.WebRequest request)
     // where TResponseData : IResponseData
        {
            using (this.Logger.BeginScope<string>(LoggerHelper.GetCaller(nameof(SendRequestInternalAsync))))
            {
                this.Logger.LogDebug(string.Format("{0} request sending...", request.Type));
                if (this._serverSocket == null || !this._serverSocket.Connected)
                {
                    Exception ex = new Exception("Socket not available");
                    this.Logger.LogWarning(ex, ex.Message);
                    return WebResponse.GetFailed(ex, request.SessionId);
                }
                this.Logger.LogDebug("Web requestData initialized");
                byte[] webRequestBytes = BinaryConverter.Serialize(request);
                this._serverSocket.Send(webRequestBytes, 0, webRequestBytes.Length, SocketFlags.None);
                this.Logger.LogDebug("Request has been sent");
                byte[] receiveBytes = new byte[this.Options.BufferSize];
                ArraySegment<byte> receiveBuffer = new ArraySegment<byte>(receiveBytes);
                Task<int> task1 = this._serverSocket.ReceiveAsync(receiveBuffer, SocketFlags.None);
                Task task2 = await Task.WhenAny((Task)task1, Task.Delay(this.Options.StandartReceiveTimeout));
                bool timeout = task2 != task1;
                task2 = (Task)null;
                if (timeout)
                {
                    receiveBuffer = null;
                    receiveBytes = null;
                    Exception ex = new Exception(string.Format("Timeout: {0} ms.", (object)this.Options.StandartReceiveTimeout.TotalMilliseconds));
                    return WebResponse.GetFailed(ex, request.SessionId);
                }
                int received = task1.Result;
                if (received == 0)
                {
                    receiveBuffer = null;
                    receiveBytes = null;
                    Exception exception = new Exception(string.Format("No reply received, bytes: {0}", (object)received));
                    this.Logger.LogError(exception, exception.Message);
                    return WebResponse.GetFailed(exception, request.SessionId);
                }
                ArraySegment<byte> correctBytes = new ArraySegment<byte>(receiveBuffer.Array ?? Array.Empty<byte>(), 0, received);
                WebResponse webResponse = BinaryConverter.Deserialize<WebResponse>(correctBytes.Array);
                correctBytes = null;
                receiveBuffer = null;
                receiveBytes = null;
                if (request.RequestId == webResponse.CallingRequestId)
                    return webResponse;
                Exception exception1 = new Exception("RequestId not equal to ResponseId");
                return WebResponse.GetFailed(exception1, request.RequestId);
            }
        }
        public async Task<WebResponse> SendRequestInternalAsync(
      Core.Abstractions.WebRequest request, TimeSpan timeSpan)
        // where TResponseData : IResponseData
        {
            using (this.Logger.BeginScope<string>(LoggerHelper.GetCaller(nameof(SendRequestInternalAsync))))
            {
                this.Logger.LogDebug(string.Format("{0} request sending...", request.Type));
                if (this._serverSocket == null || !this._serverSocket.Connected)
                {
                    Exception ex = new Exception("Socket not available");
                    this.Logger.LogWarning(ex, ex.Message);
                    return WebResponse.GetFailed(ex, request.SessionId);
                }
                this.Logger.LogDebug("Web requestData initialized");
                byte[] webRequestBytes = BinaryConverter.Serialize(request);
                this._serverSocket.Send(webRequestBytes, 0, webRequestBytes.Length, SocketFlags.None);
                this.Logger.LogDebug("Request has been sent");
                byte[] receiveBytes = new byte[this.Options.BufferSize];
                ArraySegment<byte> receiveBuffer = new ArraySegment<byte>(receiveBytes);
                Task<int> task1 = this._serverSocket.ReceiveAsync(receiveBuffer, SocketFlags.None);
                Task task2 = await Task.WhenAny((Task)task1, Task.Delay(timeSpan));
                bool timeout = task2 != task1;
                task2 = (Task)null;
                if (timeout)
                {
                    receiveBuffer = null;
                    receiveBytes = null;
                    Exception ex = new Exception(string.Format("Timeout: {0} ms.", timeSpan.TotalSeconds));
                    return WebResponse.GetFailed(ex, request.SessionId);
                }
                int received = task1.Result;
                if (received == 0)
                {
                    receiveBuffer = null;
                    receiveBytes = null;
                    Exception exception = new Exception(string.Format("No reply received, bytes: {0}", (object)received));
                    this.Logger.LogError(exception, exception.Message);
                    return WebResponse.GetFailed(exception, request.SessionId);
                }
                ArraySegment<byte> correctBytes = new ArraySegment<byte>(receiveBuffer.Array ?? Array.Empty<byte>(), 0, received);
                WebResponse webResponse = BinaryConverter.Deserialize<WebResponse>(correctBytes.Array);
                correctBytes = null;
                receiveBuffer = null;
                receiveBytes = null;
                if (request.RequestId == webResponse.CallingRequestId)
                    return webResponse;
                Exception exception1 = new Exception("RequestId not equal to ResponseId");
                return WebResponse.GetFailed(exception1, request.RequestId);
            }
        }
        private Core.Abstractions.WebRequest GenerateRequest(byte[] requestData, string requestType) => new Core.Abstractions.WebRequest()
        {
            Data = requestData,
            Type = requestType,
            RequestId = Guid.NewGuid().ToString(),
            SessionId = this.SessionId
        };

        /*---------------------------------------------------------------------------------------------------------------------*/
        #region Control

        public async Task<WebResponse> SendRequestAsyncControl(
byte[] requestData, string requestType, TimeSpan timeSpan)
//where TResponseData : IResponseData
        {
            Core.Abstractions.WebRequest request = this.GenerateRequest(requestData, requestType);
            return await this.SendRequestInternalAsync(request, timeSpan);
        }
        public async Task<WebResponse> SendRequestAsyncControl(
           byte[] requestData, string requestType, int Count, TimeSpan timeSpan)
           //where TResponseData : IResponseData
        {
            Core.Abstractions.WebRequest request = this.GenerateRequest(requestData, requestType);
            return await this.SendRequestAsyncTryAsync(request, Count, true, timeSpan);
        }
        public async Task<WebResponse> SendRequestAsyncControl(
           byte[] requestData, string requestType, int Count)
           //where TResponseData : IResponseData
        {
            Core.Abstractions.WebRequest request = this.GenerateRequest(requestData, requestType);
            return await this.SendRequestAsyncTryAsync(request, Count, false); ;
        }

        private async Task<WebResponse> SendRequestAsyncTryAsync
            (Core.Abstractions.WebRequest Request, int Count, bool TimeOut ,TimeSpan timeSpan = default(TimeSpan))
            //where TResponseData : IResponseData
        {
            Core.Abstractions.WebRequest request = Request;
            WebResponse response = new WebResponse();
            string type = request.Type;//request.GetType().Name;
            for (int i = 0; i < Count + 1; i++)
            {
                
                if (TimeOut)
                {
                    response = await this.SendRequestInternalAsync(request, timeSpan);
                }
                else
                {
                    response = await this.SendRequestInternalAsync(request);
                }


                if (response.Exception != null)
                {
                    this.Logger.LogWarning("Fail send " + type + " TryCount: " + i + " ex: " + response.Exception);
                    if (i == Count)
                    {
                        this.Logger.LogError("Fail send " + type + " : " + response.Exception);
                        request = (Core.Abstractions.WebRequest)null;
                        throw response.Exception;
                    }
                    await Task.Delay(1000);
                }
                else
                {
                    if (response.Data != null)
                    {
                        /*
                        if (!response.Data.Ok)
                        {
                            this.Logger.LogWarning("Send " + type + " success but an error occurred on the other side. TryCount: " + i);
                            if (i == Count)
                            {
                                this.Logger.LogError("Send " + type + " success but an error occurred on the other side");
                                response.Exception = new Exception("error occurred on the other side");
                                //throw new Exception("error occurred on the other side");
                            }
                            await Task.Delay(1000);
                        }
                        else
                        {
                            this.Logger.LogDebug("Send " + type + " success");
                            request = (Core.Abstractions.WebRequest)null;
                            return response;
                        }
                        */
                        this.Logger.LogDebug("Send " + type + " success");
                        request = (Core.Abstractions.WebRequest)null;
                        return response;
                    }
                    else
                    {
                        this.Logger.LogWarning("Send " + type + " WTF?! response data == null. TryCount: " + i);
                        if (i == Count)
                        {
                            this.Logger.LogError("Send " + type + " WTF?! response data == null, fuck this shit");
                            response.Exception = new Exception("error occurred onno derrucco rorre");
                            //throw new Exception("error occurred on the other side");
                        }
                        await Task.Delay(1000);
                    }
                }
            }
            request = null;
            Request = null;
            type = null;
            return response;
        }



        #endregion

        /*---------------------------------------------------------------------------------------------------------------------*/
        public void Dispose()
        {
            this._serverSocket?.Dispose();
        }
        public void SetPort(int port)
        {
            this.Options.ServerPort = port;
        }
    }
}
