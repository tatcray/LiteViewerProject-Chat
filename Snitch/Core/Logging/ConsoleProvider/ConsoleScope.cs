using System;
using System.Collections.Generic;
using System.Linq;

namespace Snitch.Core.Logging.ConsoleProvider
{
    internal class ConsoleScope : IDisposable
    {
        private static string _localScope;

        private static List<string> _scopes { get; set; } = new List<string>();

        public ConsoleScope(string scope)
        {
            ConsoleScope._localScope = scope;
            ConsoleScope._scopes.Add(ConsoleScope._localScope);
        }

        public static List<string> Scopes => ConsoleScope._scopes.ToList<string>();

        public void Dispose() => ConsoleScope._scopes.Remove(ConsoleScope._localScope);

        private static string GetScope()
        {
            if (ConsoleScope._scopes.Count == 0)
                return "";
            string str = "";
            foreach (string scope in ConsoleScope._scopes)
                str = str + "[" + scope + "]";
            return str.Substring(1, str.Length - 2);
        }
    }
}
