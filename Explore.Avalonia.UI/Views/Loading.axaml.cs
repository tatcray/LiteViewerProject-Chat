using System;
using System.Threading.Tasks;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using Explore.Avalonia.UI.ViewModels;


namespace Explore.Avalonia.UI.Views
{
    public partial class Loading : Window
    {
        public Loading()
        {
            DataContext = new LoadingViewModel(this);
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

    }
}