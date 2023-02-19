using System;

namespace Explore.Avalonia.UI.ViewModels
{
    public interface IErrorHandler
    {
        void HandleError(Exception ex);
    }
}