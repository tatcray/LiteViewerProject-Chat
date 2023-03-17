using Avalonia.Controls;
using Avalonia.Interactivity;
using Explore.Avalonia.UI.ViewModels;


namespace Explore.Avalonia.UI.Views
{
    public partial class CancelAppeal : Window
    {
        public delegate void CancelAllWindowsHandler();
        public event CancelAllWindowsHandler? CancelAllWindow;
        public CancelAppeal()
        {
            DataContext = new CancelAppealViewModel(this);
            InitializeComponent();
        }

        private void CancelAll(object? sender, RoutedEventArgs e)
        {
            CancelAllWindow.Invoke();
        }
    }
}