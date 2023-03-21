using Avalonia.Controls;
using Avalonia.Interactivity;
using Explore.Avalonia.UI.ViewModels;


namespace Explore.Avalonia.UI.Views
{
    public partial class ConnectionRequest : Window
    {
        public ConnectionRequest()
        {
            DataContext = new ConnectionRequestViewModel(this);
            InitializeComponent();
        }
    }
}