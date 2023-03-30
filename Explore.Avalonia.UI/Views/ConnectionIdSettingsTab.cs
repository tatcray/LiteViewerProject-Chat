using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Explore.Avalonia.UI.Views
{
    public partial class ConnectionIdSettingsTab : UserControl
    {
        public ConnectionIdSettingsTab()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
