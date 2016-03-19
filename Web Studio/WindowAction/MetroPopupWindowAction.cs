using System;
using System.Windows;
using Prism.Interactivity;
using Prism.Interactivity.InteractionRequest;

namespace Web_Studio.WindowAction
{
    /// <summary>
    ///     Override the default PopWindowAction to use a metro window (mahapps)
    /// </summary>
    public class MetroPopupWindowAction : PopupWindowAction
    {
        /// <summary>
        ///     it creates a new metro window instead of a window
        /// </summary>
        /// <returns></returns>
        protected override Window CreateWindow()
        {
            return new MetroPopupWindow();
        }

        private new Window CreateDefaultWindow(INotification notification)
        {
            Window window = null;

            if (notification is IConfirmation)
            {
                window = new MetroConfirmationWindow {Confirmation = (IConfirmation) notification};
            }
            else
            {
                window = new MetroNotificationWindow {Notification = notification};
            }

            return window;
        }

        /// <summary>
        ///     Returns the window to display as part of the trigger action.
        /// </summary>
        /// <param name="notification">The notification to be set as a DataContext in the window.</param>
        /// <returns></returns>
        protected override Window GetWindow(INotification notification)
        {
            Window wrapperWindow;

            if (WindowContent != null)
            {
                wrapperWindow = CreateWindow();

                if (wrapperWindow == null)
                    throw new NullReferenceException("CreateWindow cannot return null");

                // If the WindowContent does not have its own DataContext, it will inherit this one.
                wrapperWindow.DataContext = notification;
                wrapperWindow.Title = notification.Title;

                PrepareContentForWindow(notification, wrapperWindow);
            }
            else
            {
                wrapperWindow = CreateDefaultWindow(notification);
            }

            // If the user provided a Style for a Window we set it as the window's style.
            if (WindowStyle != null)
                wrapperWindow.Style = WindowStyle;

            return wrapperWindow;
        }
    }
}