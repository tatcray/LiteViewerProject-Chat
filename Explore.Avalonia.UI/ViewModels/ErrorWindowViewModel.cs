using System.Collections.Generic;
using Avalonia.Controls;
using System.Reactive;
using Avalonia.Controls;
using ExCSS;
using ReactiveUI;

namespace Explore.Avalonia.UI.ViewModels
{
    public class ErrorWindowViewModel : ViewModelBase
    {
        public ErrorWindowViewModel (Window window)
        {
            //RequestingTechnicalSupport window = new RequestingTechnicalSupport();
            //window.Show();
            this.CloseWindow = ReactiveCommand.Create(() => window.Close());
        }
        public ReactiveCommand<Unit, Unit> CloseWindow { get; set; }

    }
}