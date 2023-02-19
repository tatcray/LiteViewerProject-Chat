using Snitch.Client.Events.Args;
using System.Threading.Tasks;

namespace Snitch.Client.Events.Handlers
{
    public delegate Task DisconnectedEventler(object sender, DisconnectedEventArgs args);
}
