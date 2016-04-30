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
    }
}