using System;
using Avalonia;
using Avalonia.Controls;

public class CustomTextBox : TextBox
{
    public static readonly StyledProperty<string> PlaceholderTextProperty =
        AvaloniaProperty.Register<CustomTextBox, string>(nameof(PlaceholderText));

    public string PlaceholderText
    {
        get => GetValue(PlaceholderTextProperty);
        set => SetValue(PlaceholderTextProperty, value);
    }

    public CustomTextBox()
    {
        this.PropertyChanged += CustomTextBox_PropertyChanged;
    }

    private void CustomTextBox_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property == TextProperty)
        {
            TextChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public event EventHandler? TextChanged;
}