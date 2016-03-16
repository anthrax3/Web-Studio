using System.Windows;
using Prism.Interactivity;
using Web_Studio.Events;

namespace Web_Studio.WindowAction
{
    /// <summary>
    /// Override the default PopWindowAction to use a metro window (mahapps)
    /// </summary>
    public class MetroPopupWindowAction : PopupWindowAction
    {
        /// <summary>
        /// it creates a new metro window instead of a window
        /// </summary>
        /// <returns></returns>
        protected override Window CreateWindow()
        {
            return new MetroPopupWindow();

        }
    }
}