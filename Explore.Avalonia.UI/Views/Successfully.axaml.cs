using Avalonia.Controls;
using Avalonia.Interactivity;
using Explore.Avalonia.UI.ViewModels;


namespace Explore.Avalonia.UI.Views
{
    public partial class Successfully : Window
    {
        public delegate void CancelAllWindowsHandler();
        public event CancelAllWindowsHandler? CancelAllWindow;
        public Successfully()
        {
            DataContext = new SuccessfullyViewModel(this);
            InitializeComponent();
        }
        
        private void CancelAll(object? sender, RoutedEventArgs e)
        {
            CancelAllWindow.Invoke();
        }
    }
}