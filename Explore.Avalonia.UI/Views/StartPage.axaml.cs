using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Interactivity;
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
    }
}