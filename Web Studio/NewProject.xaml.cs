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
using Microsoft.Win32;
using Xceed.Wpf.Toolkit;

namespace Web_Studio
{
    /// <summary>
    /// Lógica de interacción para NewProject.xaml
    /// </summary>
    public partial class NewProject
    {
        public NewProject()
        {
            InitializeComponent();
        }

        private void ButtonBrowser_OnClick(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = tbProjectName.Text.Length != 0 ? tbProjectName.Text : "Projecto";
            saveFileDialog.DefaultExt = ".ws";
            saveFileDialog.Filter = "Web Studio (.ws) | *.ws ";
            saveFileDialog.ShowDialog();
        }
    }
}
