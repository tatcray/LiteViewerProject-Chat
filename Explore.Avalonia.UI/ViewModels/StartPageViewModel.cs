
using System.Collections.Generic;
using Avalonia.Controls;
using Explore.Avalonia.UI.Views;

namespace Explore.Avalonia.UI.ViewModels
{
    public class StartPageViewModel : ViewModelBase
    {
        public StartPageViewModel ()
        {
            RequestingTechnicalSupport window = new RequestingTechnicalSupport();
            window.Show();
        }

        public object ListDateTime { get; } = new List<string>
        {
            "Hôm qua",
            "Hôm nay",
            "Tháng trước",
            "Đầu tuần tới nay",
            "Đầu tháng tới nay",
            "7 ngày qua",
             "15 ngày qua",
             "30 ngày qua",
             "90 ngày qua"
        };
    }
}