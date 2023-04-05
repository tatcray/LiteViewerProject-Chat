using Avalonia.Controls;
using Avalonia.Interactivity;
using Explore.Avalonia.UI.ViewModels;


namespace Explore.Avalonia.UI.Views
{
    public partial class FileSender : Window
    {
        public FileSender()
        {
            DataContext = new FileSenderViewModel(this);
            InitializeComponent();
        }
    }
}