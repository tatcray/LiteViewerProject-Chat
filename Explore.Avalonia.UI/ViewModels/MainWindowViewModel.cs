using Avalonia.Controls;
using Avalonia.Logging;
using P2PSimpleChat.BLL;
using Snitch.Client;
using Snitch.Client.Entities;
using Snitch.Core.Abstractions.Client;
using Snitch.Core.Abstractions.Server;
using Snitch.Core.Models;
using Snitch.Server;
using Splat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Explore.Avalonia.UI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        public MainWindowViewModel ()
        {
            //SendCommand = new AsyncCommand(OnSend);
            ConnectCommand = new AsyncCommand(OnConnect);
            Task.Run(() => InitializeAsync());
        }
        public string Greeting => "Welcome to Avalonia!";

        public ObservableCollection<Message> Messages { get; } = new();

        public string _messageText { get; set; }
        public bool _IsMeSender { get; set; }

        public bool IsMeSender
        {
            get => _IsMeSender;
            set
            {
                _IsMeSender = value;
                OnPropertyChanged(nameof(IsMeSender));
            }
        }
        public string MessageText
        {
            get => _messageText;
            set
            {
                _messageText = value;
                OnPropertyChanged(nameof(MessageText));
            }
        }
        // ICommand SendCommand { get; }

        public ICommand ConnectCommand { get; }

        WebClient Client { get; set; } 
        WebServer WebServer { get; set; }
        private async Task InitializeAsync()
        {
            try
            {
                WebServer = await InitializeWebServerAsync(Messages); // �����
                WebServer.Start(); 
                Client = await InitializeWebClientAsync(); // �����
                Client.WorkingAsync();
                //������ ������ ����� �� ������� ���� �� �������� 
                //� ����� ��� �� ������ ��� �������� ����������� ����
               // await Client.SendMessageAsync(MessageText);
            }
            catch(Exception err)
            {

            }
        }
        /* ������
        public async Task SendCommand(SpecialCommands command)
        {
            try
            {
                SendCommandRequest RequestData = new SendCommandRequest()
                {
                    Command = command
                };
                WebResponse WebResponse = await WebClient.SendRequestAsyncControl(BinaryConverter.Serialize(RequestData),
                 RequestData.GetType().Name, 3, TimeSpan.FromSeconds(3));
                if (WebResponse.Exception != null)
                    throw WebResponse.Exception;

                SendCommandResponse Response = BinaryConverter.Deserialize<SendCommandResponse>(WebResponse.Data);
                if (!string.IsNullOrWhiteSpace(Response.ErrorString))
                    throw new Exception("|OS|" + Response.ErrorString);

                this.Logger.LogTrace("SendCommand success");
            }
            catch (Exception err)
            {
                this.Logger.LogError("SendCommand fail: " + err.Message);
            }
        }
        */
        private static async Task<WebServer> InitializeWebServerAsync(ObservableCollection<Message> messages)//ILogger logger)
        {
            var options = new DefaultServerOptions(12345) //����-------
            {
                BufferSize = 1024 * 1024 * 60,
                PingTimeout = TimeSpan.FromMinutes(1),
            };
            //var logger = LogSettings.LoggerFactory.CreateLogger("Server");
            return new WebServer(options, messages);//, logger);
        }
        private static async Task<WebClient> InitializeWebClientAsync()//ILogger logger)
        {
            int Port;
            Port = 1234; //----------------
            var serverIpv4 = "127.0.0.1";//IpManager.GetLocalIp();

            var options = new DefaultWebClientOptions(serverIpv4, Port)
            {
                StandartReceiveTimeout = TimeSpan.FromSeconds(20),
                MaxResponseTimeout = TimeSpan.FromSeconds(60),
                BufferSize = 1024 * 1024 * 1,//AppConfig.ApiServiceWebBuffer
            };
            var webClient = new WebClient(options);//, logger);
            webClient.InitializeAsync();
            //Logger.LogDebug("InitializeWebClientAsync() completed");
            return webClient;
        }
        public static Client GetClientObject(string ip, string port)
        {
            try
            {
                TcpClient client = new TcpClient();
                client.Connect(new System.Net.IPEndPoint(System.Net.IPAddress.Parse(ip), int.Parse(port)));
                return new Client(client);
            }
            catch
            {
                throw;
            }
        }

        private async Task OnConnect()
        {
 //           _client = GetClientObject("127.0.0.1", "1234");
        }

        //        private async Task OnServerConnection()
        //        {

        //        }

        public ICommand SendCommand => new AsyncCommand(async () =>
        {
            try
            {
                if (string.IsNullOrEmpty(MessageText))
                    return;
                var Message = new Message() { MessageText = MessageText, Sender = "CLient2", IsMeSender = true };
                await Client.SendMessageAsync(Message);
                Messages.Add(Message);

                MessageText = "";
                IsMeSender= false;
                await Task.Delay(1000);
            }
            catch (Exception ex)
            {
               // ErrorViewHelper.ShowError(ex);
            }
        });

    /*    private async Task OnSend()
        {
            if (string.IsNullOrEmpty(MessageText))
               return;

           await Client.SendMessageAsync(MessageText);

            MessageText = "";
        }*/
    } 
}
