using Avalonia.Controls;
using Avalonia.Interactivity;
using Explore.Avalonia.UI.ViewModels;


namespace Explore.Avalonia.UI.Views
{
    public partial class ScreenConnection : Window
    {
        public ScreenConnection()
        {
            DataContext = new ScreenСonnectionViewModel(this);
            InitializeComponent();
        }

        private void ChatButton_OnClick(object? sender, RoutedEventArgs e)
        {
            ChatWindow window = new ChatWindow();
            window.Show();
        }
    }
}