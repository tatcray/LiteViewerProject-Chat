using System.Threading.Tasks;

namespace Snitch.Core.Abstractions.Server
{
    public interface IFinishMaster
    {
        Task MasterFinish(string TransactionGuid);
        Task<string> AskWhere(string fileName);
    }
}
