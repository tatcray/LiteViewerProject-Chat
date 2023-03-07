using System.Collections.ObjectModel;
using System.ComponentModel;
using Avalonia.Controls;
using Explore.Avalonia.UI.ViewModels;
using ReactiveUI;

namespace Explore.Avalonia.UI.Views
{
    public partial class StartPage : Window, IReactiveObject
    {
        private ObservableCollection<string> _options = new ObservableCollection<string>();

        public StartPage()
        { 
            // Add some options to the collection
            Options.Add("Option 1");
            Options.Add("Option 2");
            Options.Add("Option 3");
            // Add more options here...
            
            InitializeComponent();
        }

        public ObservableCollection<string> Options
        {
            get { return _options; }
            set { this.RaiseAndSetIfChanged(ref _options, value); }
        }

        public event PropertyChangingEventHandler? PropertyChanging;
        public void RaisePropertyChanging(PropertyChangingEventArgs args)
        {
            throw new System.NotImplementedException();
        }

        public void RaisePropertyChanged(PropertyChangedEventArgs args)
        {
            throw new System.NotImplementedException();
        }
    }
}
