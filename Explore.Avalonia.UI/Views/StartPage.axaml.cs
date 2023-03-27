using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Explore.Avalonia.UI.ViewModels;
using ReactiveUI;


namespace Explore.Avalonia.UI.Views
{
    public partial class StartPage : Window, IReactiveObject
    {
        private ObservableCollection<string> _options = new ObservableCollection<string>();

        public StartPage()
        {
            DataContext = new StartPageViewModel();
            InitializeComponent();
        }

     
        public event PropertyChangingEventHandler? PropertyChanging;
        public void RaisePropertyChanging(PropertyChangingEventArgs args)
        {
            throw new System.NotImplementedException();
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            myComboBox.IsDropDownOpen = true;
        }

        public void RaisePropertyChanged(PropertyChangedEventArgs args)
        {
            throw new System.NotImplementedException();
        }

        private void ScreenСonnection(object? sender, RoutedEventArgs e)
        {
            ScreenConnection window = new ScreenConnection();
            window.Show();
        }

        private void OnSupportButtonClick(object? sender, PointerPressedEventArgs e)
        {
            RequestingTechnicalSupport window = new RequestingTechnicalSupport();
            window.Show();
        }

        private void OnSettingsButtonClicked(object? sender, RoutedEventArgs e)
        {
            SettingsId window = new SettingsId();
            window.Show();
        }
    }
}