using Snitch.Client.Events.Args;
using System.Threading.Tasks;

namespace Snitch.Client.Events.Handlers
{
    public delegate Task WorkSpaceEventler(object sender, WorkSpaceEventArgs args);
}
