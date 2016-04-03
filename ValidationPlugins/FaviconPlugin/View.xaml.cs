using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FaviconPlugin.Properties;

namespace FaviconPlugin
{
    /// <summary>
    ///     Lógica de interacción para View.xaml
    /// </summary>
    public partial class View : UserControl
    {
        /// <summary>
        ///     Default constructor
        /// </summary>
        /// <param name="vm"></param>
        public View(Favicon vm)
        {
            InitializeComponent();
            DataContext = vm;
        }

        /// <summary>
        ///     Method to manage the drop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UIElement_OnDrop(object sender, DragEventArgs e)
        {
            var files = (string[]) e.Data.GetData(DataFormats.FileDrop);
            if (files.Length > 1)
            {
                tbInfo.Text = Strings.MoreThanOneFile;
            }
            else
            {
                var extension = Path.GetExtension(files[0]);
                switch (extension)
                {
                    case ".png":
                    case ".jpg":
                    case ".JPG":
                    case ".PNG":
                        ((Favicon) DataContext).PathToImage = files[0];
                        tbInfo.Text = "";
                        myGrid.Background = new ImageBrush(new BitmapImage(new Uri(files[0])));
                        break;
                    default:
                        tbInfo.Text = Strings.BadExtension;
                        myGrid.Background = null;
                        break;
                }
            }
        }
    }
}