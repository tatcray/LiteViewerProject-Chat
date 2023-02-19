using System.Runtime.CompilerServices;

namespace Snitch.Core
{
    public static class LoggerHelper
    {
        public static string GetCaller([CallerMemberName] string caller = null) => caller;

        public static string GetFailed([CallerMemberName] string caller = null) => caller + " failed";
    }
}
