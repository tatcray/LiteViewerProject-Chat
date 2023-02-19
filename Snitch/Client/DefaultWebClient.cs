using Microsoft.Extensions.Logging;
using Snitch.Client.Events.Args;
using Snitch.Client.Events.Handlers;
using Snitch.Core.Abstractions.Client;
using System.Threading.Tasks;

namespace Snitch.Client
{
    public class DefaultWebClient : WebClient
    {
        public DefaultWebClient(IWebClientOptions options)
          : base(options)
          => this.Logger = (ILogger)new LoggerFactory().CreateLogger<DefaultWebClient>();

        public DefaultWebClient(IWebClientOptions options, ILogger logger)
          : base(options)
          => this.Logger = logger;

        public bool InWork { get; private set; } = false;

        public bool IsConnected { get; private set; } = false;

        protected ILogger Logger { get; }

        public event WorkSpaceEventler OnWorkSpaceAsync;

        public event ConnectedEventler OnConnectedAsync;

        public event DisconnectedEventler OnDisconnectedAsync;

        public async Task WorkingAsync()
        {
            this.InWork = true;
            while (this.InWork)
            {
                await Task.Delay(2000);
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
                if (this.OnWorkSpaceAsync != null)
                    await this.OnWorkSpaceAsync((object)this, new WorkSpaceEventArgs());
            }
        }

        public void Abort()
        {
            this.InWork = false;
            this.Logger.LogDebug("Web client aborted");
        }

        private async Task ManageDisconnectAsync()
        {
            if (!this.IsConnected)
                return;
            this.IsConnected = false;
            if (this.OnDisconnectedAsync != null)
                await this.OnDisconnectedAsync((object)this, new DisconnectedEventArgs());
        }

        private async Task ManageConnectionAsync()
        {
            if (this.IsConnected)
                return;
            this.IsConnected = true;
            if (this.OnConnectedAsync != null)
                await this.OnConnectedAsync((object)this, new ConnectedEventArgs());
        }
    }
}
