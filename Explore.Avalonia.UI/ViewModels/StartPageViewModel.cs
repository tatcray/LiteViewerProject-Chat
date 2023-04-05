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
            //FileSender window = new FileSender(); 
            //window.Show();
            
        }
        
        public void OpenNewWindow(object sender, PointerPressedEventArgs e)
        {
            SettingsId window = new SettingsId(); 
            window.Show(); 
        }
        
        public IEnumerable<object> ItemsSource { get; } = new List<object> { "Item 1", "Item 2", "Item 3" };
    }
}