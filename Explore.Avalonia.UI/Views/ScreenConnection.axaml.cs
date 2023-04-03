using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Explore.Avalonia.UI.ViewModels;


namespace Explore.Avalonia.UI.Views
{
    public partial class ScreenConnection : Window
    {
        public ScreenConnection()
        {
            DataContext = new ScreenСonnectionViewModel(this);
            InitializeComponent();
        }

        
        private void OnPointerPressed(object sender, PointerPressedEventArgs e)
        {
            var slider = this.FindControl<StackPanel>("slider");

            if (slider.IsVisible && e.Source != slider && e.Source != this.FindControl<Button>("showSlider"))
            {
                slider.IsVisible = false;
            }
        }

        /*private void ShowToolTip_PointerEnter(object sender, PointerEventArgs e)
        {
            var toolTip = this.FindControl<Border>("toolTip");

            toolTip.IsVisible = true;
        }
        
        private void HideToolTip_PointerLeave(object sender, PointerEventArgs e)
        {
            var toolTip = this.FindControl<Border>("toolTip");

            toolTip.IsVisible = false;
        } */
        
        private void ShowSlider_Click(object sender, RoutedEventArgs e)
        {
            var slider = this.FindControl<StackPanel>("slider");

            slider.IsVisible = !slider.IsVisible;
        }
        
        private void ChatButton_OnClick(object? sender, RoutedEventArgs e)
        {
            ChatWindow window = new ChatWindow();
            window.Show();
        }
    }
}