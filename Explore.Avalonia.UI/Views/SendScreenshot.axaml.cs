using Avalonia.Controls;
using Avalonia.Interactivity;
using Explore.Avalonia.UI.ViewModels;


namespace Explore.Avalonia.UI.Views
{
    public partial class SendScreenshot : Window
    {
        public delegate void CancelAllWindowsHandler();
        public event CancelAllWindowsHandler? CancelAllWindow;
        public SendScreenshot()
        {
            DataContext = new SendScreenshotViewModel(this);
            InitializeComponent();
        }
        
        private void Button_YesOrNo_Click(object? sender, RoutedEventArgs e)
        {
            Successfully window = new Successfully();
            window.CancelAllWindow += OnCancelWindowOk;
            window.Show();   
        }
        
        private void OnCancelWindowOk()
        {
            CancelAllWindow.Invoke();
            this.Close();
        }
        private void Button_Cancel_Click(object? sender, RoutedEventArgs e)
        {
            CancelAppeal window = new CancelAppeal();
            window.Show();        
        }
    }
}