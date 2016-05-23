using System.Windows;
using System.Windows.Controls;

namespace FtpClient
{
    /// <summary>
    ///     FTPClient portable class
    /// </summary>
    public class FtpClient : Control
    {
        static FtpClient()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FtpClient), new FrameworkPropertyMetadata(typeof(FtpClient)));
        }

        /// <summary>
        /// Dependency property of Local Path
        /// </summary>
        public static readonly DependencyProperty LocalPathProperty =
       DependencyProperty.Register("LocalPath", typeof(string), typeof(FtpClient), new FrameworkPropertyMetadata("",FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,LocalPathChanged));

        /// <summary>
        /// Local path changed handler
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void LocalPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as FtpClient;
            if (control != null) control.LocalPath = (string)e.NewValue;
        }

        /// <summary>
        /// Selected text (can be bindable)
        /// </summary>
        public string LocalPath
        {
            get { return (string)GetValue(LocalPathProperty); }
            set
            {
                SetValue(LocalPathProperty, value);
                ViewModel.Instance.LocalPath = value;
                if (value == null)
                {
                    ViewModel.Instance.InitLocal();
                }
                else
                {
                    ViewModel.Instance.GetLocalFilesAndFolders(ViewModel.Instance.LocalItems,value);
                }
            }
        }

    }
}