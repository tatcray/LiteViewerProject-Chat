using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Explore.Avalonia.UI.Views
{
    public partial class WindowHeader : UserControl
    {
        public WindowHeader()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public string Title
        {
            get { return (this.FindControl<TextBlock>("TitleTextBlock")).Text; }
            set { (this.FindControl<TextBlock>("TitleTextBlock")).Text = value; }
        }

        public Button MinimizeButton
        {
            get { return this.FindControl<Button>("minimizeButton"); }
        }

        public Button MaximizeButton
        {
            get { return (Button)this.FindControl<Button>("maximizeButton"); }
        }

        public Button CloseButton
        {
            get { return (Button)this.FindControl<Button>("closeButton"); }
        }
    }
}