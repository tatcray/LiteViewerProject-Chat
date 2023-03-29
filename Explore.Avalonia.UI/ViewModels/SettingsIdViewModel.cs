using System.Reactive;
using Avalonia.Controls;
using ReactiveUI;

namespace Explore.Avalonia.UI.ViewModels;

public class SettingsIdViewModel : ViewModelBase
{
    public string testte => "ASDAS";
    
    public SettingsIdViewModel (Window window)
    {
        //RequestingTechnicalSupport window = new RequestingTechnicalSupport();
        //window.Show();
        this.CloseWindow = ReactiveCommand.Create(() => window.Close());
    }
    public ReactiveCommand<Unit, Unit> CloseWindow { get; set; }
}