using System;
using System.Collections.Generic;
using System.Linq;

namespace Snitch.Core.Logging.FileProvider
{
    internal class FileScope : IDisposable
    {
        private static string _localScope;
        private static readonly List<string> _scopes = new List<string>();

        public FileScope(string scope)
        {
            FileScope._localScope = scope;
            FileScope._scopes.Add(FileScope._localScope);
        }

        public static List<string> Scopes => FileScope._scopes.ToList<string>();

        public void Dispose() => FileScope._scopes.Remove(FileScope._localScope);

        private static string GetScope()
        {
            if (FileScope._scopes.Count == 0)
                return "";
            string str = "";
            foreach (string scope in FileScope._scopes)
                str = str + "[" + scope + "]";
            return str.Substring(1, str.Length - 2);
        }
    }
}
