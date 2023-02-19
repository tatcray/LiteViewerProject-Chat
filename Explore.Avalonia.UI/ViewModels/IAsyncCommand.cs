using System.Threading.Tasks;
using System.Windows.Input;

namespace Explore.Avalonia.UI.ViewModels
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync();
        bool CanExecute();
    }
}