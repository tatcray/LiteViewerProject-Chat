using Snitch.Core.Abstractions.Server;
using System.Net;
using System.Net.Sockets;
using BinaryPack;
using Snitch.Client;
using Snitch.Core.Abstractions;
using Snitch.Core.Web.Request;
using Snitch.Core.Web.Response;
using Snitch.Server.Events.Args;
using Snitch.Server.Events.lers;
using WebResponse = Snitch.Core.Abstractions.WebResponse;

namespace Snitch.Server
{
    public class WebServer : IWebServerAsync, IDisposable
    {
        private byte[] _buffer;
//        private readonly List<ClientMetaData> _connectedClients = new List<ClientMetaData>();
        protected Socket Socket;

        /*---------------------------------------------------------------------------------------------------------------------*/
//        public IEnumerable<ClientMetaData> ConnectedClients => (IEnumerable<ClientMetaData>)this._connectedClients.ToArray();

        /*---------------------------------------------------------------------------------------------------------------------*/

//        public event ClientConnectedEventler ClientConnected;

//        public event ClientDisconnectedEventler ClientDisconnected;

//        public event PingedEventler Pinged;

        /*---------------------------------------------------------------------------------------------------------------------*/
        public WebServer(IWebServerOptions options)
        {
            this.InternalLogger = (ILogger)new LoggerFactory().CreateLogger<WebServer>();
            this.Options = options;
        }
        public WebServer(IWebServerOptions options, ILogger internalLogger)
        {
            this.InternalLogger = internalLogger;
            this.Options = options;
        }
        /*---------------------------------------------------------------------------------------------------------------------*/
        public IWebServerOptions Options { get; }

        public bool IsStarted { get; private set; }
        public int Port { get; private set; }

        private ILogger InternalLogger { get; set; }

        /*---------------------------------------------------------------------------------------------------------------------*/
        /*
        public IResponseData ManageRequest(
  IRequestData requestData,
  Socket clientSocket,
  string sessionId)
        {

        }
        */
        public WebResponse ManageRequest(
  Snitch.Core.Abstractions.WebRequest webRequest,
  Socket clientSocket)
        {
            switch (webRequest.Type)
            {
                case "PingRequest":
                    return this.ManagePingRequest(webRequest, clientSocket);
                case "SendClientInfoRequest":
                    return this.ManageClientInfoRequest(
                        BinaryConverter.Deserialize<SendClientInfoRequest>(webRequest.Data), webRequest.SessionId);
                case "TurnClientRequest":
                    return this.ManageTurnClientsRequest(
                        BinaryConverter.Deserialize<TurnClientRequest>(webRequest.Data), webRequest.SessionId);
                case "WhatRequest":
                    return this.AnswerWhatRequest(webRequest.SessionId);
                case "SendConnectStringRequest":
                    return this.ManageSendConnectStringRequest(
                        BinaryConverter.Deserialize<SendConnectStringRequest>(webRequest.Data), webRequest.SessionId);
//                case "GetUserConnection":
//                  return this.GetUserConnection(
                default:
                    throw new Exception($"Can't initialize request for: " +
                                         $"{webRequest.Type}");// $"{webRequest.Data.GetType()}");
            }
        }








        /*---------------------------------------------------------------------------------------------------------------------*/

        private WebResponse ManageClientInfoRequest(SendClientInfoRequest data, string sessionId)
        {
            SendClientInfoResponse response = new SendClientInfoResponse();
            try
            {
                
                ClientMetaData clientMetaData = this._connectedClients.FirstOrDefault(item => item.SessionId == sessionId);

                if (clientMetaData == null)
                {
                    response.Ok = false;
                    response.ErrorString = "clientMetaData == null";
                }
                else
                {
                    clientMetaData.ID = data.ClientInfo.Id;
                    clientMetaData.ClientOnline = data.ClientOnline;
                    response.ConnectedClients = this.ConnectedClients.Count(); //GetConnectedClients(); //нужно переделать
                    response.Ok = true;
                }
                
            }
            catch(Exception err)
            {
                this.InternalLogger.LogError("ManageClientInfoRequest error: " + err.Message);
                response.Ok = false;
                response.ErrorString = err.Message;
            }
            return new WebResponse()
            {
                Data = BinaryConverter.Serialize(response),
                Type = response.GetType().Name
            };
        }

        private List<ClientInfo> GetConnectedClients()
        {
            List<ClientInfo> ConnectedClients = new List<ClientInfo>();
            foreach (ClientMetaData client in this._connectedClients)
            {
                if (client != null)
                {
                    ClientInfo C = new ClientInfo
                    {
                        Id = client.ID
                    };
                    ConnectedClients.Add(C);
                }
            }
            return ConnectedClients;
        }

        private WebResponse AnswerWhatRequest(string sessionId)
        {

            ClientMetaData clientMetaData = this._connectedClients.FirstOrDefault(item => item.SessionId == sessionId);
            this.InternalLogger.LogInformation("Manage AnswerWhat to " + clientMetaData.ID);
            //дерево выбора ответа
            if (clientMetaData.WhantToConnect)
            {
                return this.ManageOpenDesktopShare(sessionId);
            }

            if (clientMetaData.GetConnectString)
            {
                return this.ManageGetConnectString(sessionId);
            }

            throw new Exception($"Can't initialize request for: " +
                                        $"AnswerWhatRequest()");

        }

        private WebResponse ManageGetConnectString(string sessionId)
        {
            ConnectStringResponse response = new ConnectStringResponse();
            try
            {
                ClientMetaData clientMetaData = this._connectedClients.FirstOrDefault(item => item.SessionId == sessionId);
                this.InternalLogger.LogInformation("Manage GetConnectString to " + clientMetaData.ID);
                
                if (clientMetaData == null)
                {
                    throw new Exception($"Can't find client: " + sessionId + " clientMetaData is null");
                }
                /*
                if (string.IsNullOrWhiteSpace(clientMetaData.ConnectString))
                {
                    //ошибка
                    //лог
                    throw new Exception($"Can't initialize responce for: " +
                                            $"ManageGetConnect()" + " ConnectString is null");
                }
                */
                response.ConnectPermission = clientMetaData.ConnectPermission;
                response.Reason = !clientMetaData.ConnectPermission ? clientMetaData.Reason : null;
                response.Error = !clientMetaData.ConnectPermission ? clientMetaData.Error : null;
                response.ConnectString = clientMetaData.ConnectString;
                response.WLConnectString = clientMetaData.WLConnectString;
                response.ClipboardServerPort = clientMetaData.ConnectPermission ? clientMetaData.ClipboardServerPort 
                    : Dilettante.Shifrovka(0);
                response.ShareTextBufferBool = clientMetaData.ShareTextBufferBool;
                clientMetaData.GetConnectString = false;
                clientMetaData.ConnectString = null;
                clientMetaData.WLConnectString = null;

                response.Ok = true;
            }
            catch (Exception err)
            {
                this.InternalLogger.LogError("ManageOpenDesktopShare error: " + err.Message);
                response.Ok = false;
                response.ErrorString = err.Message;
            }

            return new WebResponse()
            {
                Data = BinaryConverter.Serialize(response),
                Type = response.GetType().Name
            };

        }

        private WebResponse ManageOpenDesktopShare(string sessionId)
        {
            OpenDesktopShareResponse response = new OpenDesktopShareResponse();
            try
            {
                ClientMetaData ToclientMetaData = this._connectedClients.FirstOrDefault(item => item.SessionId == sessionId);
                this.InternalLogger.LogInformation("Manage OpenDesktopShare to " + ToclientMetaData.ID);
                
                if (ToclientMetaData == null)
                {
                    throw new Exception($"Can't find client: " + sessionId + " ToclientMetaData is null");
                }
                //----------------------инициатор теряется
                ClientMetaData InitiatorClientMetaData = this._connectedClients.FirstOrDefault(item => item.ID == ToclientMetaData.InitiatorID);
                
                if (InitiatorClientMetaData == null)
                {
                    throw new Exception($"Can't find Initiator: " + ToclientMetaData.InitiatorID + " InitiatorClientMetaData is null");
                }
                response.InitiatorID = ToclientMetaData.InitiatorID;
                response.AuthBool = ToclientMetaData.WhantAuth;
                response.Json = InitiatorClientMetaData.Json;
                //response.ConnectPass = InitiatorClientMetaData.ConnectPass;
                response.ShareBufferBool = InitiatorClientMetaData.ShareBufferBool;
                response.ShareTextBufferBool = InitiatorClientMetaData.ShareTextBufferBool;
                response.ShareAudioBool = InitiatorClientMetaData.ShareAudioBool;
                //response.AuthLogin = InitiatorClientMetaData.AuthLogin;
                //response.AuthPass = InitiatorClientMetaData.AuthPass; --------
                //response.InitiatorAddress = initiatorClientMetaData.ClientSocket.RemoteEndPoint.ToString();
                //response.ToAddress = ToclientMetaData.ClientSocket.RemoteEndPoint.ToString();
                ToclientMetaData.InitiatorID = null;
                ToclientMetaData.WhantToConnect = false;
                response.Ok = true;
            }
            catch(Exception err)
            {
                this.InternalLogger.LogError("ManageOpenDesktopShare error: " + err.Message);
                response.Ok = false;
                response.ErrorString = err.Message;
            }
            
            return new WebResponse()
            {
                Data = BinaryConverter.Serialize(response),
                Type = response.GetType().Name
            };
        }

        private WebResponse ManageTurnClientsRequest(TurnClientRequest requestData, string sessionId)
        {
            TurnClientResponse response = new TurnClientResponse();
            try
            {
                this.InternalLogger.LogInformation("Manage TurnClient to " + requestData.ToID);
                ClientMetaData ToClientMetaData = this._connectedClients.FirstOrDefault(item => item.ID == requestData.ToID);
                ClientMetaData InitiatorClientMetaData = this._connectedClients.FirstOrDefault(item => item.SessionId == sessionId);
                if (ToClientMetaData == null)
                {
                    //лог об ошибке
                    response.ErrorString = "Client offline";
                    response.WorkerOnline = false;
                    response.Ok = true;
                }
                else
                {
                    response.Ok = true;
                    response.WorkerOnline = true;
                    response.ClientOnline = ToClientMetaData.ClientOnline;

                    ToClientMetaData.WhantToConnect = true;
                    ToClientMetaData.InitiatorID = InitiatorClientMetaData.ID;
                    ToClientMetaData.WhantAuth = requestData.AuthBool;
                    InitiatorClientMetaData.Json = requestData.Json;
                    //InitiatorClientMetaData.ConnectPass = requestData.ToPass;
                    //InitiatorClientMetaData.AuthLogin = requestData.AuthLogin;
                    // InitiatorClientMetaData.AuthPass = requestData.AuthPass;
                    InitiatorClientMetaData.ShareBufferBool = requestData.ShareBufferBool;
                    InitiatorClientMetaData.ShareTextBufferBool = requestData.ShareTextBufferBool;
                    InitiatorClientMetaData.ShareAudioBool = requestData.ShareAudioBool;
                }
            }
            catch (Exception err)
            {
                this.InternalLogger.LogError("ManageTurnClientsRequest error: " + err.Message);
                response.ErrorString = err.Message;
                response.Ok = false;
            }

            return new WebResponse()
            {
                Data = BinaryConverter.Serialize(response),
                Type = response.GetType().Name
            };

        }

        private WebResponse ManageSendConnectStringRequest(SendConnectStringRequest data, string sessionId)
        {
            SendConnectStringResponse response = new SendConnectStringResponse();
            try
            {
                this.InternalLogger.LogInformation("Manage SendConnectString to " + data.toID);
                ClientMetaData clientMetaData = this._connectedClients.FirstOrDefault(item => item.ID == data.toID);
                /*
               if (clientMetaData == null)
               {
                   //лог ошибки
               }
               else
               {

               }
               */
                if (data.Ok)
                {
                    clientMetaData.ConnectPermission = true;
                    clientMetaData.ConnectString = data.ConnectString;
                    clientMetaData.WLConnectString = data.WLConnectString;
                    clientMetaData.GetConnectString = true;
                    clientMetaData.ClipboardServerPort = data.ClipboardServerPort;
                    clientMetaData.ShareTextBufferBool = data.ShareTextBufferBool;
                }
                else
                {
                    clientMetaData.ConnectPermission = false;
                    clientMetaData.Reason = data.Reason;
                    clientMetaData.Error = data.Error;
                    clientMetaData.GetConnectString = true;
                }

                response.Ok = true;
            }
            catch(Exception err)
            {
                this.InternalLogger.LogError("ManageSendConnectStringRequest error: " + err.Message);
                response.Ok = false;
                response.ErrorString = err.Message;
            }
            
            
            return new WebResponse() 
            { 
                Data = BinaryConverter.Serialize(response),
                Type = response.GetType().Name
            };
        }

        #region Ping
        private WebResponse ManagePingRequest(Snitch.Core.Abstractions.WebRequest request, Socket clientSocket)
        {
            PingResponse response = new PingResponse();
            try
            {
                ClientMetaData clientMetaData1 = this._connectedClients.FirstOrDefault(item => item.SessionId == request.SessionId);
                if (clientMetaData1 == null)
                {
                    ClientMetaData clientMetaData2 = new ClientMetaData(clientSocket, request.SessionId)
                    {
                        LastPingTime = DateTime.Now
                    };
                    this._connectedClients.Add(clientMetaData2);
                    ClientConnectedEventler clientConnected = this.ClientConnected;
                    if (clientConnected != null)
                        clientConnected((object)this, new ClientConnectedEventArgs()
                        {
                            SessionId = clientMetaData2.SessionId
                        });
                }
                else
                {
                    clientMetaData1.LastPingTime = DateTime.Now;
                    PingedEventler pinged = this.Pinged;
                    if (pinged != null)
                        pinged((object)this, new PingedEventArgs()
                        {
                            SessionId = clientMetaData1.SessionId
                        });
                    this.ManageClientsPingTimeout();
                    response.needWhat = (clientMetaData1.WhantToConnect || clientMetaData1.GetConnectString);
                }
                response.Ok = true;
            }
            catch(Exception err)
            {
                this.InternalLogger.LogError("ManagePingRequest error: " + err.Message);
                response.Ok = false;
                response.ErrorString = err.Message;
            }
            
            //this.ManageClientsPingTimeout();
            return new WebResponse() 
            { 
                Data = BinaryConverter.Serialize(response),
                Type = response.GetType().Name
            };

        }

        private void ManageClientsPingTimeout()
        {
            foreach (string clientSessionId in
                this._connectedClients.FindAll(item => item.LastPingTime + this.Options.PingTimeout < DateTime.Now).Select(item => item.SessionId))
                this.DisconnectClient(clientSessionId);
        }

        #endregion

        /*---------------------------------------------------------------------------------------------------------------------*/

        public void Start()
        {
            try
            {
                this.IsStarted = !this.IsStarted ? true : throw new Exception("Server already started");
                this.InternalLogger.LogTrace("Server starting...");
                this._buffer = new byte[this.Options.BufferSize];
                this.Socket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
                this.Socket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);
                this.Socket.Bind((EndPoint)new IPEndPoint(IPAddress.Any, this.Options.ServerPort));
                Port = (this.Socket.LocalEndPoint as IPEndPoint).Port;
                this.Socket.ReceiveTimeout = 10000;
                this.Socket.Listen(2000);
                this.Socket.BeginAccept(new AsyncCallback(this.AcceptCallback), (object)null);
                this.InternalLogger.LogInformation("Server started");
            }
            catch (Exception ex)
            {
                this.InternalLogger.LogError(ex, LoggerHelper.GetFailed(nameof(Start)));
                throw;
            }
        }

        private void AcceptCallback(IAsyncResult asyncResult)
        {
            Socket socket;
            try
            {
                if (this.Socket == null)
                    return;
                socket = this.Socket.EndAccept(asyncResult); //----
            }
            catch (Exception err)//(ObjectDisposedException ex)
            {
                this.InternalLogger.LogError("AcceptCallback: Error EndAccept - " + err.Message);
                return;
            }

            try
            {
                socket.BeginReceive(this._buffer, 0, this._buffer.Length, SocketFlags.None, new AsyncCallback(this.ReceiveCallback), (object)socket); //--
            }
            catch (Exception err)
            {
                this.InternalLogger.LogError("AcceptCallback: Error while receiving data - " + err.Message);
                this.InternalLogger.LogWarning("AcceptCallback: Receiving data were ignored");
                //return;
            }

            this.Socket.BeginAccept(new AsyncCallback(this.AcceptCallback), (object)null);
        }

        private void ReceiveCallback(IAsyncResult asyncResult)
        {
            Socket current = (Socket)asyncResult.AsyncState;
            try
            {
                int length;
                try
                {
                    length = current.EndReceive(asyncResult);
                    if(length == 0)
                    {
                        DisconnectClient(current);
                        return;
                    }
                }
                catch (SocketException ex)
                {
                    this.InternalLogger.LogWarning("ReceiveCallback: EndReceive SocketException - " + ex.Message);
                    DisconnectClient(current);
                    return;
                }
                byte[] requestBytes = new byte[length];
                Array.Copy((Array)this._buffer, (Array)requestBytes, length);
                WebResponse response = this.GetResponse(requestBytes, current);
                current.Send(BinaryConverter.Serialize(response));
                current.BeginReceive(this._buffer, 0, this._buffer.Length, SocketFlags.None, new AsyncCallback(this.ReceiveCallback), (object)current);
            }
            catch (Exception ex)
            {
                this.InternalLogger.LogError("ReceiveCallback: " + ex.Message);
                try
                {
                    current.BeginReceive(this._buffer, 0, this._buffer.Length, SocketFlags.None, new AsyncCallback(this.ReceiveCallback), (object)current);
                }
                catch (Exception err)
                {
                    this.InternalLogger.LogError("ReceiveCallback: current.BeginReceive - " + ex.Message);
                    DisconnectClient(current);
                }
                
            }
        }

        private WebResponse GetResponse(
      byte[] requestBytes,
      Socket clientSocket)
        {
            Core.Abstractions.WebRequest webRequest = requestBytes.Length != 0 
                ? BinaryConverter.Deserialize<Core.Abstractions.WebRequest>(requestBytes) : throw new Exception("Request bytes: 0");
            //this.InternalLogger.LogInformation(string.Format("New request: " + webRequest.Type);
            try
            {
                WebResponse response = this.ManageRequest(webRequest, clientSocket);
                response.CallingRequestId = webRequest.RequestId;
                return response;
                /*
                return new WebResponse<IResponseData>()
                {
                    CallingRequestId = webRequest.RequestId,
                    Data = responseData
                };
                */
            }
            catch (Exception ex)
            {
                this.InternalLogger.LogError("GetResponse error: " + ex.Message);
                //this.InternalLogger.LogError(ex, ex.Message);
                return WebResponse.GetFailed(ex, webRequest.RequestId);
            }
        }

        /*---------------------------------------------------------------------------------------------------------------------*/

        protected void DisconnectClient(string clientSessionId)
        {
            ClientMetaData clientMetaData = this._connectedClients.FirstOrDefault(item => item.SessionId == clientSessionId);
            if (clientMetaData == null)
                return;
            if (clientMetaData.ClientSocket == null)
                return;
            clientMetaData.ClientSocket.Close();
            this._connectedClients.Remove(clientMetaData);
            this.InternalLogger.LogWarning("DisconnectClient: Force client disconnection");
            ClientDisconnectedEventArgs args = new ClientDisconnectedEventArgs()
            {
                SessionId = clientSessionId
            };
            ClientDisconnectedEventler clientDisconnected = this.ClientDisconnected;
            if (clientDisconnected != null)
                clientDisconnected((object)this, args);
        }
        protected void DisconnectClient(Socket current)
        {
            ClientMetaData clientMetaData = this._connectedClients.FirstOrDefault(item => item.ClientSocket == current);
            if (clientMetaData != null)
            {
                this._connectedClients.Remove(clientMetaData);
                this.InternalLogger.LogWarning("ReceiveCallback: ClientMetaData disconnected");
                ClientDisconnectedEventArgs args = new ClientDisconnectedEventArgs()
                {
                    SessionId = clientMetaData.SessionId
                };
                ClientDisconnectedEventler clientDisconnected = this.ClientDisconnected;
                if (clientDisconnected != null)
                    clientDisconnected((object)this, args);
                this.InternalLogger.LogWarning("ReceiveCallback: Event invoked");
            }
            current.Close();
        }

        /*---------------------------------------------------------------------------------------------------------------------*/

        public void Dispose()
        {
            this.Socket?.Dispose();
            this.Socket = (Socket)null;
        }
    }
}
