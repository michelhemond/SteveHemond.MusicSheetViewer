using Prism.Interactivity;
using SteveHemond.MusicSheetViewer.Controls;
using System.Windows;

namespace SteveHemond.MusicSheetViewer.Infrastructure
{
    public class MetroPopupWindowAction : PopupWindowAction
    {
        protected override Window CreateWindow()
        {
            return new MetroPopupWindow();
        }
    }
}