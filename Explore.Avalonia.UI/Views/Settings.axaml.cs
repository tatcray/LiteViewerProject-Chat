using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using Avalonia;
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
        private Button button1;
        private Button button2;
        public SettingsId()
        {
            DataContext = new SettingsIdViewModel(this);
            InitializeComponent();
            
            button1 = this.FindControl<Button>("TabSelectionButton1");
            button2 = this.FindControl<Button>("TabSelectionButton2");
            
            button1.Click += Button1_Click;
            button2.Click += Button2_Click;
        }
        
        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            button1.Foreground = Brush.Parse("#3A8DDE");
            button2.Foreground = Brush.Parse("#7C7C7C");
            
            ContentControl.Content = new ConnectionIdSettingsTab();
        }
        
        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            button2.Foreground = Brush.Parse("#3A8DDE");
            button1.Foreground = Brush.Parse("#7C7C7C");

            var buttonSender = (Button)sender;
            buttonSender.Classes.Clear();

            ContentControl.Content = new WhiteListSettingsTab();

        }

        private void SetSelectedTab(Button tab)
        {
            //получить все кнопки с тэгом TabSelectionButton
            //убрать у всех кнопок класс SelectedListText, если он есть
            //поставить на кнопку из аргумента метода, класс SelectedListText
        }

        private void InputElement_OnGotFocus(object? sender, GotFocusEventArgs e)
        {
            var thisButton = sender;
            var btn = ((Button)sender);
            if (thisButton != sender)
            {
                btn.Background = Brushes.Transparent;
                btn.Margin = new Thickness(20, 10, 0, 0);
                btn.Foreground = Brush.Parse("#7C7C7C");
                btn.FontSize = 16; 
            }
            else
            {
             btn.Background = Brushes.Transparent;
            btn.Margin = new Thickness(20, 10, 0, 0);
            btn.Foreground = Brush.Parse("#3A8DDE");
            btn.FontSize = 16;   
            }

        }
    }
}
