using System;

namespace Snitch.Core.Abstractions
{
    [Serializable]
    public class WebRequest
    {
        public byte[] Data { get; set; }
        public string Type { get; set; }

        public string RequestId { get; set; }

        public string SessionId { get; set; }
    }
}
