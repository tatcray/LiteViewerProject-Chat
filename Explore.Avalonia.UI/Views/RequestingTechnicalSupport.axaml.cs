using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Explore.Avalonia.UI.ViewModels;
using ReactiveUI;

namespace Explore.Avalonia.UI.Views
{
    public partial class RequestingTechnicalSupport : Window
    {
        private SendScreenshot window;
        public RequestingTechnicalSupport()
        {
            InitializeComponent();
        }
        
        public void TextBlock_OnPointerPressed(object sender, PointerPressedEventArgs e)
        {
            TextBlock textBlock = (TextBlock)sender;
            string link = "https://www.zeluslugi.ru/how-we-are-working/helpdesk";
            Process.Start(new ProcessStartInfo(link)
            {
                UseShellExecute = true
            });
        }

        private void Logo_OnPointerPressed(object? sender, RoutedEventArgs routedEventArgs)
        {
            string link = "https://www.zeluslugi.ru/";
            Process.Start(new ProcessStartInfo(link)
            {
                UseShellExecute = true
            });
        }

        private void Button_Cancel_Click(object? sender, RoutedEventArgs e)
        {
            CancelAppeal window = new CancelAppeal();
            window.CancelAllWindow += OnCancelWindowOk;
            window.Show();        
        }

        private void OnCancelWindowOk()
        {
            this.Close();
        }

        private void Button_Send_Click(object? sender, RoutedEventArgs e)
        {
            SendScreenshot window = new SendScreenshot();
            window.CancelAllWindow += OnCancelWindowOk;
            window.Show();        
        }
    }
}
