using Avalonia.Controls;
using Avalonia.Interactivity;
using Explore.Avalonia.UI.ViewModels;


namespace Explore.Avalonia.UI.Views
{
    public partial class ErrorWindow : Window
    {
        public ErrorWindow()
        {
            DataContext = new ErrorWindowViewModel(this);
            InitializeComponent();
        }
    }
}