using Snitch.Server.Events.Args;

namespace Snitch.Server.Events.lers
{
    public delegate void ClientDisconnectedEventler(
      object sender,
      ClientDisconnectedEventArgs args);
}
