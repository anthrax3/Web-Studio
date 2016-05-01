using System;
using System.Windows;
using Web_Studio.Events;
using Xceed.Wpf.AvalonDock.Layout;

namespace Web_Studio
{
    /// <summary>
    ///     Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        ///     Default constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            messagesContainer.ToggleAutoHide();
            var root = (LayoutAnchorablePane)messagesContainer.Parent;
            root.DockHeight = new GridLength(200);
            EventSystem.Subscribe<ChangedLanguageEvent>(ManageChangedLanguage);
            EventSystem.Subscribe<MessageContainerVisibilityChangedEvent>(MessageContainerVisibilityChanged);
        }

        /// <summary>
        /// Update visiblity of container
        /// </summary>
        /// <param name="obj"></param>
        private void MessageContainerVisibilityChanged(MessageContainerVisibilityChangedEvent obj)
        {
            try
            {
                messagesContainer.IsActive = IsVisible;

            }
            catch (Exception e)
            {
              Telemetry.Telemetry.TelemetryClient.TrackException(e);
            }
        }

        /// <summary>
        /// Show explorer control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExplorerMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            ExplorerLayout.Show();
        }

        /// <summary>
        /// Method to manage the changed language event
        /// </summary>
        /// <param name="obj"></param>
        private void ManageChangedLanguage(ChangedLanguageEvent obj)
        {
            BusyControl.RefreshUI();
            MessageListControl.RefreshUI();
        }
    }
}