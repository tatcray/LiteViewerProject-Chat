using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Explore.Avalonia.UI.Views;

public partial class WhiteListSettingsTab : UserControl
{
    public WhiteListSettingsTab()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}