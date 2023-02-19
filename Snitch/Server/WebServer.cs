using BinaryPack;
using Microsoft.Extensions.Logging;
using Snitch.Client;
using Snitch.Core;
using Snitch.Core.Abstractions;
using Snitch.Core.Abstractions.Server;
using Snitch.Core.Models;
using Snitch.Core.Utilities;
using Snitch.Core.Web.Request;
using Snitch.Core.Web.Response;
using Snitch.Server.Events.Args;
using Snitch.Server.Events.lers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using WebResponse = Snitch.Core.Abstractions.WebResponse;

namespace Snitch.Server
{
    public class WebServer : IWebServerAsync, IDisposable
    {
        private byte[] _buffer;
        private readonly List<ClientMetaData> _connectedClients = new List<ClientMetaData>();
        protected Socket Socket;

        /*---------------------------------------------------------------------------------------------------------------------*/
        public IEnumerable<ClientMetaData> ConnectedClients => (IEnumerable<ClientMetaData>)this._connectedClients.ToArray();

        /*---------------------------------------------------------------------------------------------------------------------*/

        public event ClientConnectedEventler ClientConnected;

        public event ClientDisconnectedEventler ClientDisconnected;

        public event PingedEventler Pinged;

        /*---------------------------------------------------------------------------------------------------------------------*/
        public WebServer(IWebServerOptions options, ObservableCollection<Message> messages)
        {
            this.InternalLogger = (ILogger)new LoggerFactory().CreateLogger<WebServer>();
            this.Options = options; 
            this.Messages = messages;
        }
        public WebServer(IWebServerOptions options, ILogger internalLogger, ObservableCollection<Message> messages)
        {
            this.InternalLogger = internalLogger;
            this.Options = options;
            this.Messages = messages;
        }
        /*---------------------------------------------------------------------------------------------------------------------*/
        public IWebServerOptions Options { get; }

        public ObservableCollection<Message> Messages { get; }

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
                    return this.ManagePingRequest(webRequest);
                case "GetMaessageRequest":
                    return this.ManageMessage(webRequest);
                default:
                    throw new Exception($"Can't initialize request for: " +
                                         $"{webRequest.Type}");// $"{webRequest.Data.GetType()}");
            }
        }








        /*---------------------------------------------------------------------------------------------------------------------*/





        #region Ping
        private WebResponse ManagePingRequest(Snitch.Core.Abstractions.WebRequest request)
        {
            PingResponse response = new PingResponse();
            try
            {
                //тут ты что то делаешь с реквестом

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


        #region Message
        private WebResponse ManageMessage(Snitch.Core.Abstractions.WebRequest request)
        {
            var requestObj = BinaryConverter.Deserialize<GetMaessageRequest>(request.Data);
            this.Messages.Add( requestObj.Message); 
            GetMaessageResponse response = new GetMaessageResponse();
            try
            {
                response.Ok = true;
            }
            catch (Exception err)
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
                this.Socket.SetIPProtectionLevel(IPProtectionLevel.Unrestricted);
                this.Socket.Bind((EndPoint)new IPEndPoint(IPAddress.IPv6Any, Options.ServerPort));
                Port = (this.Socket.LocalEndPoint as IPEndPoint).Port;
                this.Socket.Listen(10);
                this.Socket.BeginAccept(new AsyncCallback(this.AcceptCallback), (object)null);

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
