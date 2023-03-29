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
    public partial class SettingsId : Window
    {
        public SettingsId()
        {
            DataContext = new SettingsIdViewModel(this);
            InitializeComponent();
        }
    }
}
