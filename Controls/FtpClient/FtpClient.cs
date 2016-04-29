using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FtpClient.Protocols.ItemTypes;
using FtpClient.Protocols.Messages;

namespace FtpClient
{
        
    /// <summary>
    ///  FTPClient portable class
    /// </summary>
    public class FtpClient : Control
    {
        static FtpClient()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FtpClient), new FrameworkPropertyMetadata(typeof(FtpClient)));
        }
    }
}
