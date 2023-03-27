using Avalonia.Controls;
using Avalonia.Interactivity;
using Explore.Avalonia.UI.ViewModels;


namespace Explore.Avalonia.UI.Views
{
    public partial class NewVersion : Window
    {
        public NewVersion()
        {
            DataContext = new NewVersionViewModel(this);
            InitializeComponent();
        }
    }
}