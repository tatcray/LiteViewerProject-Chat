using System.Collections.Generic;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Input;
using Explore.Avalonia.UI.Views;

namespace Explore.Avalonia.UI.ViewModels
{
    public class StartPageViewModel : ViewModelBase
    {
        public StartPageViewModel ()
        {
            //RequestingTechnicalSupport window = new RequestingTechnicalSupport();
            //window.Show();
            
        }
        
        public void OpenNewWindow(object sender, PointerPressedEventArgs e)
        {
            RequestingTechnicalSupport window = new RequestingTechnicalSupport(); 
            window.Show(); 
        }
        
        public IEnumerable<object> ItemsSource { get; } = new List<object> { "Item 1", "Item 2", "Item 3" };
    }
}