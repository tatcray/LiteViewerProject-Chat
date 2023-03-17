using System.Collections.Generic;
using System.Reactive;
using Avalonia.Controls;
using ExCSS;
using ReactiveUI;

namespace Explore.Avalonia.UI.ViewModels
{
    public class SendScreenshotViewModel : ViewModelBase
    {
        public SendScreenshotViewModel (Window window)
        {
            //RequestingTechnicalSupport window = new RequestingTechnicalSupport();
            //window.Show();
            this.CloseWindow = ReactiveCommand.Create(() => window.Close());
        }

        public SendScreenshotViewModel()
        {
        }

        public ReactiveCommand<Unit, Unit> CloseWindow { get; set; }
        
    }
}