using Avalonia.Controls;
using Explore.Avalonia.UI.ViewModels;

namespace Explore.Avalonia.UI.Views
{
    public partial class ChatWindow : Window
    {
        public ChatWindow()
        {
            DataContext = new ChatWindowViewModel();
            InitializeComponent();
        }
    }
}