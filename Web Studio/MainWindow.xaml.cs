using System.Windows;
using Microsoft.Win32;
using Web_Studio.Models;
using Web_Studio.ViewModels;

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
        }

        /// <summary>
        ///     Modal window to manage the options
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Options_OnClick(object sender, RoutedEventArgs e)
        {
          //  var optionWindow = new Options();
           // optionWindow.Show();
        }

        /// <summary>
        ///     Display a wizard to config a new project
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileNewProject_OnClick(object sender, RoutedEventArgs e)
        {
            var newProjectWindow = new NewProject {Owner = this};
            newProjectWindow.ShowDialog();
        }

        private void ExplorerMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            ExplorerLayout.Show();
        }
    }
}