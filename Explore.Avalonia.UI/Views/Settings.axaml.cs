using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Explore.Avalonia.UI.ViewModels;
using ReactiveUI;

namespace Explore.Avalonia.UI.Views
{
    public partial class SettingsId : Window
    {
        public SettingsId()
        {
            DataContext = new SettingsIdViewModel(this);
            InitializeComponent();
        }

        public void ShowWhiteListTab(object sender, RoutedEventArgs e)
        {
            ContentControl.Content = new WhiteListSettingsTab();
        }

        public void ShowConnectionTab(object sender, RoutedEventArgs e)
        {
            var buttonSender = (Button)sender;
            buttonSender.Classes.Clear();
            ContentControl.Content = new ConnectionIdSettingsTab();
        }

        private void SetSelectedTab(Button tab)
        {
            //получить все кнопки с тэгом TabSelectionButton
            //убрать у всех кнопок класс SelectedListText, если он есть
            //поставить на кнопку из аргумента метода, класс SelectedListText
        }

        private void InputElement_OnGotFocus(object? sender, GotFocusEventArgs e)
        {
            ((Button)sender).Background = Brushes.Transparent;
            ((Button)sender).Foreground = Brush.Parse("#3A8DDE");
        }
    }
}
