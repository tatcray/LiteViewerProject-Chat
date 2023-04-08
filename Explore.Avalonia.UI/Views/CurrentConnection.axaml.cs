using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Win32;
using ExCSS;
using Explore.Avalonia.UI.ViewModels;


namespace Explore.Avalonia.UI.Views
{
    public partial class CurrentConnection : Window
    {
        private bool isWindowMinimized = false;
        private double previousWidth;
        private double previousHeight;
        private PixelPoint previousPosition;
        public CurrentConnection()
        {
            this.CornerRadius = new CornerRadius(10);
            this.PositionWindowAtScreenBottomRight(this);
            DataContext = new CurrentConnectionViewModel(this);
            InitializeComponent();
        }
        
        public void PositionWindowAtScreenBottomRight(Window window)
        {
            window.Topmost = true;
            var screens = Screens.All.ToList();
            if (screens.Count > 0)
            {
                var screenBounds = screens[0].Bounds;
                var newPosition = new PixelPoint(
                    (int)(screenBounds.Right - 270), //ToDo: динамически передавать высоту и ширину
                    (int)(screenBounds.Bottom - 240));
                window.Position = newPosition;
            }
        }
        private void OnResizeButtonClick(object sender, RoutedEventArgs e)
        {
            global::Avalonia.Svg.Skia.Svg svg = this.FindControl<global::Avalonia.Svg.Skia.Svg>("DisabledSvg");
            TextBlock textBlock = this.FindControl<TextBlock>("DisabledText");
            Button button = this.FindControl<Button>("ResizeButton");
            
            global::Avalonia.Svg.Skia.Svg arrowSvg = button.FindControl<global::Avalonia.Svg.Skia.Svg>("RotatingSvg");
            RotateTransform rotateTransform = new RotateTransform();

            if (!isWindowMinimized) // Если окно не было уменьшено
            {
                previousWidth = this.Width; // Сохраняем ширину окна
                previousPosition = this.Position; // Сохраняем текущую позицию окна
                previousHeight = this.Height; // Сохраняем высоту окна
                this.Width = 50; // Устанавливаем новую ширину окна
                this.Height = 50; // Устанавливаем новую высоту окна
                this.Position = new PixelPoint(previousPosition.X + 200, previousPosition.Y + 100); // меняем позицию
                isWindowMinimized = true; // Устанавливаем флаг, что окно было уменьшено

                button.Margin = new Thickness(-9, 0, 11, 6);
                textBlock.IsVisible = false;
                svg.IsVisible = false;
                rotateTransform.Angle = 180;
            }
            else // Если окно уже было уменьшено
            {
                this.Width = previousWidth; // Возвращаем исходную ширину окна
                this.Height = previousHeight; // Возвращаем исходную высоту окна
                this.Position = previousPosition; // Возвращаем исходную позицию окна
                isWindowMinimized = false; // Сбрасываем флаг, что окно было уменьшено
                
                button.Margin = new Thickness(0);
                textBlock.IsVisible = true;
                svg.IsVisible = true;
                rotateTransform.Angle = 0;
            }
            arrowSvg.RenderTransform = rotateTransform;
        }
        
    }
}