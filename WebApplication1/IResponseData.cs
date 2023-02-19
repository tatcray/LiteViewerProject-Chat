
namespace Snitch.Core.Abstractions
{
    public abstract class IResponseData
    {
        public bool Ok { get; set; }

        public string ErrorString { get; set; }
    }
}
