using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Svg.Skia;
using Svg;
using SvgImage = Avalonia.Svg.Skia.SvgImage;

namespace Explore.Avalonia.UI.Views;

public partial class WhiteListSettingsTab : UserControl
{

    public TextBox inputTextBox;
    private StackPanel mainPanel;
    public WhiteListSettingsTab()
    {
        InitializeComponent();
        mainPanel = this.Find<StackPanel>("MainPanel");
        inputTextBox = this.Find<TextBox>("InputTextBox");
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void AddItem(object sender, RoutedEventArgs e)
    {
        StackPanel disabledStackPanel = this.FindControl<StackPanel>("DisabledStackPanel");
        disabledStackPanel.IsVisible = !disabledStackPanel.IsVisible;
    }
    
    private void OnAddButtonClick(object sender, RoutedEventArgs e)
    {
        // Создаем новый элемент StackPanel
        var stackPanel = new StackPanel()
        {
            Orientation = Orientation.Horizontal
        };

        // Получаем Svg из ресурсов приложения

        SvgSource svg = new SvgSource();
        svg.Load("../../../Assets/Settings/BlueTrash.svg");
        // Создаем новый элемент Svg
        SvgImage svgImage = new SvgImage()
        {
            Source = svg,
        };


        // Создание элемента Image и добавление в него SvgImage
        var image = new Image()
        {
            Source = svgImage,
            Width=14,
            Height=16,
            Cursor = new Cursor(StandardCursorType.Hand)
        };

        var button = new Button()
        {
            Width = 22,
            Height = 22,
            Margin=new Thickness(8),
            Padding= new Thickness(3),
            Background = Brushes.Transparent,
            Content = image
        };


        var textBlock = new TextBlock()
        {
            Text = inputTextBox.Text,
            Margin=new Thickness(0, 8, 0, 0),
            FontFamily = new FontFamily("Open Sans"),
            FontSize = 14
        };

        // Добавляем Svg и текст в StackPanel
        stackPanel.Children.Add(button);
        stackPanel.Children.Add(textBlock);
        
        button.Click += (sender, e) =>
        {
            stackPanel.Children.Remove(button);
            stackPanel.Children.Remove(textBlock);
        };

        // Добавляем новый StackPanel в главный StackPanel
        mainPanel.Children.Add(stackPanel);
    }
}